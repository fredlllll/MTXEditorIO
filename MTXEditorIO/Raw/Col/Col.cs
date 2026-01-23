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
            baseVertOffset = (Marshal.SizeOf<ColHeader>() + Marshal.SizeOf<ColObjectHeader>() * header.numObjects + 15) & 0xFFFFFFF0; //16 byte aligned
            baseIntensityOffset = baseVertOffset + header.totalLargeVerts * Marshal.SizeOf<ColVertex>() + header.totalSmallVerts * Marshal.SizeOf<ColSmallVertex>();
            baseFaceOffset = (baseIntensityOffset + header.totalVerts + 3) & 0xFFFFFFFC;//4 byte aligned
            baseBSPOffset = (baseFaceOffset + header.totalSmallFaces * Marshal.SizeOf<ColSmallFace>() + header.totalLargeFaces * Marshal.SizeOf<ColFace>()+3) & 0xFFFFFFFC;
            //TODO: bsp aligment
        }
    }
}
