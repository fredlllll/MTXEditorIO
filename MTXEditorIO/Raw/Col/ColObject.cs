using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MTXEditorIO.Raw.Col
{
    public class ColObject
    {
        public long headerOffset; //used for writing header once offsets are known
        public ColObjectHeader header;
        public IColVertex[] vertices = Array.Empty<IColVertex>();
        public byte[] intensities = Array.Empty<byte>();
        public IColFace[] faces = Array.Empty<IColFace>();
        public ColBSPTree bspTree = new ColBSPTree();

        public void ReadAllData(BinaryReader reader, ColOffsets offsets)
        {
            //now read each objects data using the offsets
            //vertices
            reader.BaseStream.Position = offsets.baseVertOffset + header.firstVertOffset;
            ReadVerts(reader);
            //intensities, one byte per vertex
            reader.BaseStream.Position = offsets.baseIntensityOffset + header.intensityOffset;
            ReadIntensities(reader);
            //faces
            reader.BaseStream.Position = offsets.baseFaceOffset + header.firstFaceOffset;
            ReadFaces(reader);
            //bsp tree
            reader.BaseStream.Position = offsets.baseBSPOffset + header.bspTreeHeadOffset;
            ReadBSP(reader);
        }

        public void ReadVerts(BinaryReader reader)
        {
            vertices = new IColVertex[header.numVerts];
            for (int j = 0; j < vertices.Length; ++j)
            {
                if (header.useSmallVerts)
                {
                    vertices[j] = reader.ReadStruct<ColSmallVertex>();
                }
                else
                {
                    vertices[j] = reader.ReadStruct<ColVertex>();
                }
            }
        }

        public void ReadIntensities(BinaryReader reader)
        {
            intensities = reader.ReadBytes(header.numVerts);
        }

        public void ReadFaces(BinaryReader reader)
        {
            faces = new IColFace[header.numFaces];
            for (int j = 0; j < faces.Length; ++j)
            {
                if (header.useSmallFaces)
                {
                    faces[j] = reader.ReadStruct<ColSmallFace>();
                }
                else
                {
                    faces[j] = reader.ReadStruct<ColFace>();
                }
            }
        }

        public void ReadBSP(BinaryReader reader)
        {
            bspTree = new ColBSPTree();
            bspTree.ReadFrom(reader);
        }

        public void WriteDummyHeader(BinaryWriter writer, ColOffsets offsets)
        {
            checked
            {
                //adjust some header values we can know now
                header.numVerts = (ushort)vertices.Length;
                header.numFaces = (ushort)faces.Length;
                header.useSmallFaces = faces.First() is ColSmallFace;
                header.useSmallVerts = vertices.First() is ColSmallVertex;
            }
            //calculate bounding box
            Vec4 bbMin = new Vec4() { x = float.MaxValue, y = float.MaxValue, z = float.MaxValue, w = 1 };
            Vec4 bbMax = new Vec4() { x = float.MinValue, y = float.MinValue, z = float.MinValue, w = 1 };

            foreach (var v in vertices)
            {
                var pos = v.GetPos(this);

                bbMin.x = Math.Min(bbMin.x, pos.x);
                bbMin.y = Math.Min(bbMin.y, pos.y);
                bbMin.z = Math.Min(bbMin.z, pos.z);

                bbMax.x = Math.Max(bbMax.x, pos.x);
                bbMax.y = Math.Max(bbMax.y, pos.y);
                bbMax.z = Math.Max(bbMax.z, pos.z);
            }

            // assign to header
            header.boundingBoxMin = bbMin;
            header.boundingBoxMax = bbMax;

            headerOffset = writer.BaseStream.Position;
            writer.WriteStruct(header);
        }

        public void WriteVerts(BinaryWriter writer, ColOffsets offsets)
        {
            checked
            {
                header.firstVertOffset = (uint)(writer.BaseStream.Position - offsets.baseVertOffset);
            }
            for (int j = 0; j < vertices.Length; ++j)
            {
                if (header.useSmallVerts)
                {
                    writer.WriteStruct((ColSmallVertex)vertices[j]);
                }
                else
                {
                    writer.WriteStruct((ColVertex)vertices[j]);
                }
            }
        }

        public void WriteIntensities(BinaryWriter writer, ColOffsets offsets) {
            checked
            {
                header.intensityOffset = (uint)(writer.BaseStream.Position - offsets.baseIntensityOffset);
            }
            writer.Write(intensities);
        }

        public void WriteFaces(BinaryWriter writer, ColOffsets offsets) {
            checked
            {
                header.firstFaceOffset = (uint)(writer.BaseStream.Position - offsets.baseFaceOffset);
            }
            for (int j = 0; j < faces.Length; ++j)
            {
                if (header.useSmallFaces)
                {
                    writer.WriteStruct((ColSmallFace)faces[j]);
                }
                else
                {
                    writer.WriteStruct((ColFace)faces[j]);
                }
            }
        }

        public void WriteBSP(BinaryWriter writer, ColOffsets offsets) {
            checked
            {
                header.bspTreeHeadOffset = (uint)(writer.BaseStream.Position - offsets.baseBSPOffset);
            }
            //TODO
        }

        internal void WriteFinalHeader(BinaryWriter writer)
        {
            writer.BaseStream.Position = headerOffset;
            writer.WriteStruct(header);
        }
    }
}
