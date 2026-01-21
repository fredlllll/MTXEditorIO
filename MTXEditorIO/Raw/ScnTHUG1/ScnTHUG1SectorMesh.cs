using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [Flags]
    public enum ScnTHUG1SectorMeshFlags : uint
    {
        //no known values
        unknown = 0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1SectorMeshHeader
    {
        public Vec3 center;
        public float radius;
        public Vec3 bboxMin;
        public Vec3 bboxMax;
        public ScnTHUG1SectorMeshFlags flags;
        public uint materialChecksum; //this is apparently an id into the list of materials
        public uint numLodLevels;
    }

    public class ScnTHUG1SectorMeshLodLevel : IReadableWriteable
    {
        public ushort[] vertIndices = Array.Empty<ushort>();

        public void ReadFrom(BinaryReader reader)
        {
            uint numIndices = reader.ReadUInt32();
            vertIndices = new ushort[numIndices];
            for (int i = 0; i < numIndices; i++)
            {
                vertIndices[i] = reader.ReadUInt16();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ScnTHUG1SectorMesh : IReadableWriteable
    {
        public ScnTHUG1SectorMeshHeader header;
        public ScnTHUG1SectorMeshLodLevel[] lodLevels = Array.Empty<ScnTHUG1SectorMeshLodLevel>();

        public void ReadFrom(BinaryReader reader)
        {
            header = reader.ReadStruct<ScnTHUG1SectorMeshHeader>();
            lodLevels = new ScnTHUG1SectorMeshLodLevel[header.numLodLevels];
            for(int i =0; i< lodLevels.Length; i++)
            {
                var ll = lodLevels[i] = new ScnTHUG1SectorMeshLodLevel();
                ll.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}