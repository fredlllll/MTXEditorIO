using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1Sector : IReadableWriteable
    {
        public ScnTHUG1SectorHeader header;
        public ScnTHUG1SectorBillboard billboard;
        public Vec3[] vertexPositions = Array.Empty<Vec3>();
        public Vec3[] vertexNormals = Array.Empty<Vec3>();
        public uint[] vertexWeights = Array.Empty<uint>();
        public ScnTHUG1SectorVertexBoneIndices[] bones = Array.Empty<ScnTHUG1SectorVertexBoneIndices>();
        public Vec2[][] vertexTexCoords = Array.Empty<Vec2[]>();
        public uint[] vertexColors = Array.Empty<uint>();
        public byte[] vertexWibbles = Array.Empty<byte>();
        public ScnTHUG1SectorMesh[] meshes = Array.Empty<ScnTHUG1SectorMesh>();

        public void ReadFrom(BinaryReader reader)
        {
            header = reader.ReadStruct<ScnTHUG1SectorHeader>();
            if (header.flags.HasFlag(SectorFlags.BILLBOARD_PRESENT))
            {
                billboard = reader.ReadStruct<ScnTHUG1SectorBillboard>();
            }

            uint numVerts = reader.ReadUInt32();
            uint vertexDataStride = reader.ReadUInt32(); //seems to be ignored
            vertexPositions = new Vec3[numVerts];
            for (int i = 0; i < numVerts; i++)
            {
                vertexPositions[i] = reader.ReadStruct<Vec3>();
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_NORMALS))
            {
                vertexNormals = new Vec3[numVerts];
                for (int i = 0; i < numVerts; i++)
                {
                    vertexNormals[i] = reader.ReadStruct<Vec3>();
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_WEIGHTS))
            {
                vertexWeights = new uint[numVerts];
                for (int i = 0; i < numVerts; i++)
                {
                    vertexWeights[i] = reader.ReadStruct<uint>();
                }
                bones = new ScnTHUG1SectorVertexBoneIndices[numVerts];
                for (int i = 0; i < numVerts; i++)
                {
                    bones[i] = reader.ReadStruct<ScnTHUG1SectorVertexBoneIndices>();
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_TEXCOORDS))
            {
                uint numTexCoordsPerVertex = reader.ReadUInt32();
                vertexTexCoords = new Vec2[numVerts][];
                for (int i = 0; i < numVerts; i++)
                {
                    var tcs = vertexTexCoords[i] = new Vec2[numTexCoordsPerVertex];
                    for (int j = 0; j < numTexCoordsPerVertex; j++)
                    {
                        tcs[j] = reader.ReadStruct<Vec2>();
                    }
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_COLORS))
            {
                vertexColors = new uint[numVerts];
                for (int i = 0; i < numVerts; i++)
                {
                    vertexColors[i] = reader.ReadUInt32();
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_COLOR_WIBBLES))
            {
                vertexWibbles = reader.ReadBytes((int)numVerts);
            }

            meshes = new ScnTHUG1SectorMesh[header.numMeshes];
            for (int i = 0; i < meshes.Length; i++)
            {
                var mesh = meshes[i] = new ScnTHUG1SectorMesh();
                mesh.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            header.numMeshes = (uint)meshes.Length;
            writer.WriteStruct(header);
            if (header.flags.HasFlag(SectorFlags.BILLBOARD_PRESENT))
            {
                writer.WriteStruct(billboard);
            }

            uint numVerts = (uint)vertexPositions.Length;
            writer.Write(numVerts);
            writer.Write((uint)0); //vertexDataStride, seems to be ignored
            for (int i = 0; i < numVerts; i++)
            {
                writer.WriteStruct(vertexPositions[i]);
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_NORMALS))
            {
                for (int i = 0; i < numVerts; i++)
                {
                    writer.WriteStruct(vertexNormals[i]);
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_WEIGHTS))
            {
                for (int i = 0; i < numVerts; i++)
                {
                    writer.WriteStruct(vertexWeights[i]);
                }
                for (int i = 0; i < numVerts; i++)
                {
                    writer.WriteStruct(bones[i]);
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_TEXCOORDS))
            {
                uint numTexCoordsPerVertex = (uint)vertexTexCoords[0].Length;
                writer.Write(numTexCoordsPerVertex);
                for (int i = 0; i < numVerts; i++)
                {
                    var tcs = vertexTexCoords[i];
                    for (int j = 0; j < numTexCoordsPerVertex; j++)
                    {
                        writer.WriteStruct(tcs[j]);
                    }
                }
            }

            if (header.flags.HasFlag(SectorFlags.HAS_VERTEX_COLORS))
            {
                for (int i = 0; i < numVerts; i++)
                {
                    writer.Write(vertexColors[i]);
                }
            }

            if(header.flags.HasFlag(SectorFlags.HAS_VERTEX_COLOR_WIBBLES))
            {
                writer.Write(vertexWibbles,0,(int)numVerts);
            }

            for(int i = 0; i < meshes.Length; i++)
            {
                meshes[i].WriteTo(writer);
            }
        }
    }
}