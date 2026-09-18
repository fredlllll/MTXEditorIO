using MTXEditorIO.Raw.GeomPS2;
using MTXEditorIO.Raw.ScnTHUG1;
using MTXEditorIO.Raw.Shared;

namespace GeomToScn
{
    /// <summary>
    /// Converts a PS2 level geometry file (GeomPS2) into a PC THUG1 world (ScnTHUG1).
    ///
    /// Payload decode status: the tag-framed sub-blocks inside each record's data-area block are
    /// fully traced. Confirmed tag kinds (4-byte `kind mode count suffix` records):
    ///   00 @+0x20 block header; 01 @+0x34 header continuation; 08 @+0x90 vertex-count (+ 3*count
    ///   dword at +0xA0); 09 @+0xA4 vertex stream of count * 8-byte (i32,i32) coordinate pairs
    ///   (2D, likely texcoords); 0A byte stream of unknown semantics; 0B triangle stream where
    ///   each 8 bytes are 3x s16 XYZ + u16 marker, coordinates in fixed-point /16 (values match
    ///   the owning record's radius/bounds, e.g. 5949 = 371.8 * 16).
    /// This decoder extracts tag 0B into real triangle meshes (positions scaled /16 and offset by
    /// the record position); tags 09/0A still need mapping before UV/normals can be written.
    /// </summary>
    public class GeomToScnConverter
    {
        public const byte TypeFileHeader = 0x01;
        public const byte TypeGeometry = 0x03;
        public const byte TypeGeometryFlagged = 0x43; // same structure as 0x03, extra flag bits set
        public const byte TypeReference = 0x05;

        /// <summary>Fixed-point divisor for payload coordinates (verified: radius * 16 == coord).</summary>
        public const float CoordinateScale = 16.0f;

        private readonly GeomPS2 geom;
        private readonly byte[] file;

        public GeomToScnConverter(GeomPS2 geom, byte[] file)
        {
            this.geom = geom;
            this.file = file;
        }

        /// <summary>Prints an overview of the PS2 file: record type distribution, checksum
        /// counts, geometry block boundaries and the data-area / record-table ranges.</summary>
        public void PrintAnalysis()
        {
            Console.WriteLine($"record table @0x{geom.header.recordTableOffset:X8}, {geom.records.Length} records");

            var byType = geom.records.GroupBy(r => r.type).OrderBy(g => g.Key).ToList();
            foreach (var grp in byType)
            {
                var uniq = grp.Select(r => r.checksum).Distinct().Count();
                Console.WriteLine($"  type 0x{grp.Key:X2}: {grp.Count(),5} records, {uniq} unique checksums");
            }

            // geometry blocks: sorted data offsets reveal the layout of the data area
            var geomRecs = geom.records.Where(r => r.type is TypeGeometry or TypeGeometryFlagged).ToList();
            var sorted = geomRecs.Select(r => (long)r.dataOffset).OrderBy(o => o).ToList();
            long dataEnd = geom.header.recordTableOffset;
            Console.WriteLine($"  geometry record count: {geomRecs.Count}");
            Console.WriteLine($"  data area: 0x000000 .. 0x{dataEnd:X8} ({dataEnd} bytes)");
            Console.WriteLine($"  geometry data offsets: {sorted.Count} blocks starting at 0x{sorted.First():X8}, ending at 0x{sorted.Last():X8}");
            long tableBytes = file.Length - geom.header.recordTableOffset;
            Console.WriteLine($"  record table: 0x{geom.header.recordTableOffset:X8} .. 0x{file.Length:X8} ({tableBytes} bytes = {tableBytes / GeomPS2Record.RecordSize} slots)");

            // how many of the reference records point into the record table vs data area
            int refsInTable = geom.records.Count(r => r.type == TypeReference && r.dataOffset > geom.header.recordTableOffset);
            Console.WriteLine($"  type 0x05 reference records pointing into the record table: {refsInTable}");

            // type 0x03 masters (one per unique checksum) vs instance records that reuse them
            int type03 = geom.records.Count(r => r.type == TypeGeometry);
            int type43 = geom.records.Count(r => r.type == TypeGeometryFlagged);
            int uniqueChecksums = geom.records.Where(r => r.type == TypeGeometry).Select(r => (long)r.checksum).Distinct().Count();
            Console.WriteLine($"  type 0x03: {type03} records, {uniqueChecksums} unique checksums ({type03 - uniqueChecksums} instances reuse a checksum)");
            Console.WriteLine($"  type 0x43: {type43} records, {geom.records.Where(r => r.type == TypeGeometryFlagged).Select(r => (long)r.checksum).Distinct().Count()} unique checksums");

            Console.WriteLine($"  payload blocks: {geom.records.Count(r => r.dataOffset < geom.header.recordTableOffset)} in the data area, contiguous [dataOffset_i, dataOffset_i+1)");
            DumpPayloadProbe();
        }

        /// <summary>Builds a THUG1 scene from the PS2 records. Each geometry record becomes a
        /// sector containing a single mesh shell with the recorded bounding volume; materials
        /// are the unique record checksums. Triangle data is not emitted until the PS2 payload
        /// format is decoded (see <see cref="ExtractMeshPayload"/>).</summary>
        public ScnTHUG1 ConvertToScn()
        {
            var scn = new ScnTHUG1();

            var geomRecs = geom.records
                .Where(r => r.type is TypeGeometry or TypeGeometryFlagged)
                .OrderBy(r => r.dataOffset)
                .ToList();

            // material table keyed by the PS2 record checksum (material/texture id)
            var materialByChecksum = new Dictionary<uint, ScnTHUG1Material>();
            scn.materials = Array.Empty<ScnTHUG1Material>();

            var sectors = new List<ScnTHUG1Sector>();
            foreach (var rec in geomRecs)
            {
                var sector = ConvertRecord(rec, materialByChecksum);
                sectors.Add(sector);
            }

            scn.materials = materialByChecksum.Values.ToArray();
            scn.sectors = sectors.ToArray();
            return scn;
        }

        private ScnTHUG1Sector ConvertRecord(GeomPS2Record rec, Dictionary<uint, ScnTHUG1Material> materialByChecksum)
        {
            var sector = new ScnTHUG1Sector();
            sector.header.checksum = rec.checksum;
            sector.header.boneIndex = -1;

            // size of the bounding box in engine units
            float sx = Math.Abs(rec.positionB.x);
            float sy = Math.Abs(rec.positionB.y);
            float sz = Math.Abs(rec.positionB.z);
            sector.header.boundingBoxMin = new Vec3 { x = rec.positionA.x - sx / 2, y = rec.positionA.y - sy / 2, z = rec.positionA.z - sz / 2 };
            sector.header.boundingBoxMax = new Vec3 { x = rec.positionA.x + sx / 2, y = rec.positionA.y + sy / 2, z = rec.positionA.z + sz / 2 };
            sector.header.boundingSphere = new Vec4 { x = rec.positionA.x, y = rec.positionA.y, z = rec.positionA.z, w = rec.radius };

            var mesh = new ScnTHUG1SectorMesh();
            mesh.header.center = rec.positionA;
            mesh.header.radius = rec.radius;
            mesh.header.bboxMin = sector.header.boundingBoxMin;
            mesh.header.bboxMax = sector.header.boundingBoxMax;

            var material = GetOrCreateMaterial(rec.checksum, materialByChecksum);
            mesh.header.materialChecksum = material.header.fixedHeader.checksum;

            // extract the tag-0B triangle stream into real final positions
            var (positions, indices) = ExtractMeshPayload(rec.dataOffset);
            mesh.lodLevels = new[] { new ScnTHUG1SectorMeshLodLevel { vertIndices = indices } };
            sector.vertexPositions = positions;

            sector.meshes = new[] { mesh };
            return sector;
        }

        /// <summary>
        /// Reads the triangle payload of the block at <paramref name="dataOffset"/>. Walks the
        /// tag stream from +0x20, finds tag 0B (`0B <mode> <count> 6D`), and reads each 8-byte
        /// element as (xyz from 3x s16 on the /16 fixed-point grid) + u16 marker. Emits one vertex
        /// per element, offset into record world space via record position + fixed-point unscaling,
        /// and a trivial 0,1,2-then-3-set index run.
        /// </summary>
        private (Vec3[] positions, ushort[] indices) ExtractMeshPayload(uint dataOffset)
        {
            int blockStart = (int)dataOffset;
            int next = dataOffset + 1 < geom.header.recordTableOffset && file.Length > 0
                ? FindNextBlockStart(blockStart)
                : blockStart + 4;
            int blockLen = next - blockStart;

            int triStart = -1, triCount = 0, triMode = 0;
            for (int i = 0x20; i + 4 <= blockLen; i++)
            {
                byte k = file[blockStart + i];
                if (k == 0x0B && (file[blockStart + i + 1] == 0x80 || file[blockStart + i + 1] == 0xC0))
                {
                    triStart = i + 4;
                    triCount = file[blockStart + i + 2];
                    triMode = file[blockStart + i + 1];
                    break;
                }
            }

            if (triStart < 0) return (Array.Empty<Vec3>(), Array.Empty<ushort>());

            int n = Math.Min(triCount, (blockLen - triStart) / 8);
            var positions = new Vec3[n];
            var indices = new ushort[n];
            for (int v = 0; v < n; v++)
            {
                int o = blockStart + triStart + v * 8;
                float x = BitConverter.ToInt16(file, o) / CoordinateScale;
                float y = BitConverter.ToInt16(file, o + 2) / CoordinateScale;
                float z = BitConverter.ToInt16(file, o + 4) / CoordinateScale;
                positions[v] = new Vec3 { x = x, y = y, z = z };
                indices[v] = (ushort)v;
            }

            var rec = geom.records.FirstOrDefault(r => r.dataOffset == dataOffset);
            if (rec != null)
            {
                for (int v = 0; v < n; v++)
                {
                    positions[v].x += rec.positionA.x;
                    positions[v].y += rec.positionA.y;
                    positions[v].z += rec.positionA.z;
                }
            }
            return (positions, indices);
        }

        private int FindNextBlockStart(int blockStart)
        {
            // data blocks are contiguous and sorted by dataOffset
            var next = geom.records
                .Where(r => r.type is TypeGeometry or TypeGeometryFlagged && r.dataOffset > blockStart)
                .Select(r => (long)r.dataOffset)
                .OrderBy(o => o)
                .FirstOrDefault(blockStart + 4L);
            return (int)next;
        }

        private static ScnTHUG1Material GetOrCreateMaterial(uint checksum, Dictionary<uint, ScnTHUG1Material> materialByChecksum)
        {
            if (materialByChecksum.TryGetValue(checksum, out var existing)) return existing;

            var mat = new ScnTHUG1Material();
            mat.header.fixedHeader.checksum = checksum;
            mat.header.fixedHeader.materialNameChecksum = checksum;
            mat.header.fixedHeader.materialPasses = 1;
            mat.header.fixedHeader.sorted = false;
            mat.header.fixedHeader.singleSided = false;
            mat.header.fixedHeader.noBackfaceCulling = true;
            mat.header.fixedHeader.grassify = false;
            mat.header.specularPower = 0f;

            var pass = new ScnTHUG1MaterialPass(isFirstPass: true);
            pass.header.checksum = checksum;
            pass.header.flags = MaterialPassHeaderFlags.TEXTURED | MaterialPassHeaderFlags.STATIC;
            pass.header.hasColor = false;
            pass.header.blendMode = BlendMode.DIFFUSE;
            pass.header.blendFixedAlpha = 0;
            pass.header.uAddressing = UVAdressing.Repeat;
            pass.header.vAddressing = UVAdressing.Repeat;
            pass.header.filteringMode = 0;
            mat.passes = new[] { pass };

            materialByChecksum[checksum] = mat;
            return mat;
        }

        /// <summary>
        /// Decoded payload framing (reverse engineered): every type-0x03 data block starts with a
        /// 4-byte tag stream. Confirmed tags (byte0=kind, byte1=mode 0x80/0xC0, byte2=count, byte3=suffix):
        ///   00 @+0x20  block header
        ///   01 @+0x34  header continuation
        ///   08 @+0x90  vertex-count sub-block (count echoed at +0x94 and 3*count at +0xA0)
        ///   09 @+0xA4  vertex stream, count*8 bytes, each vertex = 4 x Int16 (comp1 constant ~ -1/0;
        ///               comp0/comp2 are signed coords spanning ~4096 units per block)
        ///   0A / 0B    byte streams following; 0B is count*6 bytes of u16 pairs (triangle indices
        ///               suspected), offset computed from the next tag position.
        /// Tag 09 streams repeat the same coordinate set in blocks whose records share a checksum,
        /// so vertices are shared mesh data against a per-block material/reference.
        /// Exact interpretation (world-space position vs UV vs index table) is still being confirmed.
        /// </summary>
        public void DumpPayloadProbe()
        {
            var recs = geom.records
                .Where(r => r.type == TypeGeometry && r.dataOffset < geom.header.recordTableOffset)
                .OrderBy(r => r.dataOffset)
                .ToList();

            for (int b = 0; b < Math.Min(4, recs.Count - 1); b++)
            {
                int start = (int)recs[b].dataOffset;
                int next = (int)recs[b + 1].dataOffset;
                Console.WriteLine($"  block{b} @0x{start:X6} ({next - start} bytes): record type 0x{recs[b].type:X2} cheksum 0x{recs[b].checksum:X8} r={recs[b].radius:N1}");

                // walk tags from +0x20 (pure scan; stream lengths still being confirmed)
                int i = 0x20;
                while (i + 4 <= next - start)
                {
                    byte k = file[start + i];
                    if (!(k <= 0x0F && (file[start + i + 1] is 0x80 or 0xC0) && file[start + i + 3] is >= 0x64 and <= 0x6D))
                        { i++; continue; }
                    byte count = file[start + i + 2];
                    Console.Write($"    tag {k:X2}{file[start + i + 1]:X2} count={count} @+{i:X4} ");
                    if (k == 0x08)
                    {
                        // following dwords include the echo of the count and the "3*count" size
                        Console.Write($"next u32s: ");
                        for (int j = 0; j < 4; j++)
                        {
                            uint v = BitConverter.ToUInt32(file, start + i + 4 + j * 4);
                            Console.Write($"{v,8}");
                        }
                    }
                    else if (k == 0x09)
                    {
                        // 8-byte vertices until next tag
                        Console.Write($"-> {count} verts as s16 quads: ");
                        for (int v = 0; v < Math.Min(3, (int)count); v++)
                        {
                            int o = start + i + 4 + v * 8;
                            Console.Write($"({BitConverter.ToInt16(file, o)},{BitConverter.ToInt16(file, o + 2)},{BitConverter.ToInt16(file, o + 4)},{BitConverter.ToInt16(file, o + 6)}) ");
                        }
                    }
                    else if (k is 0x0A or 0x0B)
                    {
                        Console.Write($"-> u16: ");
                        for (int v = 0; v < Math.Min(4, (int)count); v++)
                        {
                            Console.Write($"{BitConverter.ToUInt16(file, start + i + 4 + v * 2):X4} ");
                        }
                    }
                    Console.WriteLine();
                    i += 4;
                }
                Console.WriteLine();
            }
        }
    }
}