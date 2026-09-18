using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    public class Col : IReadableWriteableFromStream
    {
        public ColHeader header;
        public ColObject[] objects = Array.Empty<ColObject>();
        public ushort[] bspFaceIndexPool = Array.Empty<ushort>(); //shared u16 face-index pool, sits right after the last object's node array

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<ColHeader>();

            var offsets = new ColOffsets(header);
            Console.WriteLine(Output.ToString(offsets));

            objects = new ColObject[header.numObjects];
            for (int i = 0; i < objects.Length; ++i)
            {
                //read object headers sequentially
                var obj = objects[i] = new ColObject();
                obj.header = reader.ReadStruct<ColObjectHeader>();
            }
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].ReadAllData(reader, offsets);
            }

            if (objects.Length == 0) return;

            // resolve every object's BSP node array and read the shared face-index pool.
            // each object's node array fills its span up to the next object's bsp start;
            // the last one extends to EOF, where the pool lives.
            long[] bspStarts = new long[objects.Length];
            for (int i = 0; i < objects.Length; ++i)
            {
                bspStarts[i] = offsets.baseBSPOffset + objects[i].header.bspTreeHeadOffset;
            }
            long eof = stream.Length;

            for (int i = 0; i < objects.Length; ++i)
            {
                long end = i + 1 < objects.Length ? bspStarts[i + 1] : eof;
                int len = (int)(end - bspStarts[i]);
                if (len <= 0) continue;

                var block = new byte[len];
                reader.BaseStream.Position = bspStarts[i];
                reader.Read(block, 0, len);

                ColBSPTree.Resolve(block, objects[i].header.numFaces, i == objects.Length - 1, out bool prefix, out int count);
                objects[i].bspTree.Populate(block, prefix ? 4 : 0, count);
            }

            var lastTree = objects[^1].bspTree;
            long poolStart = bspStarts[^1] + (lastTree.hasPrefix ? 4 : 0) + (long)lastTree.nodeCount * 8;
            long poolBytes = eof - poolStart;
            if (poolBytes > 0)
            {
                bspFaceIndexPool = new ushort[poolBytes / 2];
                reader.BaseStream.Position = poolStart;
                for (int i = 0; i < bspFaceIndexPool.Length; ++i)
                {
                    bspFaceIndexPool[i] = reader.ReadUInt16();
                }
            }
        }

        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream, Encoding.ASCII, true);

            header.version = 9;
            header.numObjects = (uint)objects.Length;
            header.totalVerts = 0;
            header.totalLargeFaces = 0;
            header.totalSmallFaces = 0;
            header.totalLargeVerts = 0;
            header.totalSmallVerts = 0;

            foreach (var obj in objects)
            {
                header.totalVerts += obj.header.numVerts;
                if (obj.header.useSmallVerts)
                {
                    header.totalSmallVerts += obj.header.numVerts;
                }
                else
                {
                    header.totalLargeVerts += obj.header.numVerts;
                }
                if (obj.header.useSmallFaces)
                {
                    header.totalSmallFaces += obj.header.numFaces;
                }
                else
                {
                    header.totalLargeFaces += obj.header.numFaces;
                }
            }

            writer.WriteStruct(header);
            var offsets = new ColOffsets(header);

            //write dummy object headers
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteDummyHeader(writer, offsets);
            }

            //write verts
            writer.PadTo(offsets.baseVertOffset);
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteVerts(writer, offsets);
            }
            //write intensities
            writer.PadTo(offsets.baseIntensityOffset);
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteIntensities(writer, offsets);
            }
            //write faces
            writer.PadTo(offsets.baseFaceOffset);
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteFaces(writer, offsets);
            }
            //write bsp
            writer.PadTo(offsets.baseBSPOffset);
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteBSP(writer, offsets);
            }
            //write the shared face-index pool (all leaf face lists live here)
            for (int i = 0; i < bspFaceIndexPool.Length; ++i)
            {
                writer.Write(bspFaceIndexPool[i]);
            }
            //write final header cause we now have the offsets set correctly
            for (int i = 0; i < objects.Length; ++i)
            {
                objects[i].WriteFinalHeader(writer);
            }
        }
    }

    public class ColOffsets
    {
        public long baseVertOffset;
        public long baseIntensityOffset;
        public long baseFaceOffset;
        public long baseBSPOffset;
        public ColOffsets(ColHeader header)
        {
            var vertOffsetUnaligned = Marshal.SizeOf<ColHeader>() + Marshal.SizeOf<ColObjectHeader>() * header.numObjects;
            baseVertOffset = (vertOffsetUnaligned + 15) & 0xFFFFFFF0; //16 byte aligned
            //intensities arent aligned
            baseIntensityOffset = baseVertOffset + header.totalLargeVerts * Marshal.SizeOf<ColVertex>() + header.totalSmallVerts * Marshal.SizeOf<ColSmallVertex>();
            var faceOffsetUnaligned = baseIntensityOffset + header.totalVerts;
            baseFaceOffset = (faceOffsetUnaligned + 3) & 0xFFFFFFFC;//4 byte aligned
            var bspOffsetUnaligned = baseFaceOffset + header.totalSmallFaces * Marshal.SizeOf<ColSmallFace>() + header.totalLargeFaces * Marshal.SizeOf<ColFace>();
            baseBSPOffset = (bspOffsetUnaligned + 7) & 0xFFFFFFF8;
            //there is another block after the bsp but i have no idea what it is

            Console.WriteLine($"vertOffset: {vertOffsetUnaligned} | {baseVertOffset} (difference: {baseVertOffset - vertOffsetUnaligned})");
            Console.WriteLine($"intensityOffset: {baseIntensityOffset}");
            Console.WriteLine($"faceOffset: {faceOffsetUnaligned} | {baseFaceOffset} (difference: {baseFaceOffset - faceOffsetUnaligned})");
            Console.WriteLine($"bspOffset: {bspOffsetUnaligned} | {baseBSPOffset} (difference: {baseBSPOffset - bspOffsetUnaligned})");
        }
    }
}
