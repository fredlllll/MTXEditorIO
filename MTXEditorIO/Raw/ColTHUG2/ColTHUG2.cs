using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    public class ColTHUG2 : IReadableWriteableFromStream
    {
        public ColTHUG2Header header;
        public ColTHUG2Object[] objects = Array.Empty<ColTHUG2Object>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<ColTHUG2Header>();

            var baseVertOffset = (Marshal.SizeOf<ColTHUG2Header>() + Marshal.SizeOf<ColTHUG2ObjectHeader>() * header.numObjects + 15) & 0xFFFFFFF0; //16 byte aligned
            var baseIntensityOffset = baseVertOffset + header.totalLargeVerts * Marshal.SizeOf<ColTHUG2Vertex>() + header.totalSmallVerts * Marshal.SizeOf<ColTHUG2FixedVertex>();
            var baseFaceOffset = (baseIntensityOffset + header.totalVerts + 3) & 0xFFFFFFFC;//4 byte aligned

            Console.WriteLine($"baseVertOffset: {baseVertOffset} baseIntensityOffset: {baseIntensityOffset} baseFaceOffset: {baseFaceOffset}");

            objects = new ColTHUG2Object[header.numObjects];
            for (int i = 0; i < objects.Length; ++i)
            {
                //read object headers sequentially
                var obj = objects[i] = new ColTHUG2Object();
                obj.header = reader.ReadStruct<ColTHUG2ObjectHeader>();
            }
            for (int i = 0; i < objects.Length; ++i)
            {
                //now read each objects data using the offsets
                var obj = objects[i];
                //vertices
                reader.BaseStream.Position = baseVertOffset + obj.header.firstVertOffset;
                obj.vertices = new IColTHUG2Vertex[obj.header.numVerts];
                for (int j = 0; j < obj.vertices.Length; ++j)
                {
                    if (obj.header.useFixedVerts == 0)
                    {
                        obj.vertices[j] = reader.ReadStruct<ColTHUG2Vertex>();
                    }
                    else
                    {
                        obj.vertices[j] = reader.ReadStruct<ColTHUG2FixedVertex>();
                    }
                }
                //intensities, one byte per vertex
                reader.BaseStream.Position = baseIntensityOffset + obj.header.intensityOffset;
                obj.intensities = reader.ReadBytes(obj.header.numVerts);
                //faces
                reader.BaseStream.Position = baseFaceOffset + obj.header.firstFaceOffset;
                obj.faces = new IColTHUG2Face[obj.header.numFaces];
                for (int j = 0; j < obj.faces.Length; ++j)
                {
                    if (obj.header.useSmallFaces == 0)
                    {
                        obj.faces[j] = reader.ReadStruct<ColTHUG2Face>();
                    }
                    else
                    {
                        obj.faces[j] = reader.ReadStruct<ColTHUG2SmallFace>();
                    }
                }
            }
        }

        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            throw new NotImplementedException();
        }
    }
}
