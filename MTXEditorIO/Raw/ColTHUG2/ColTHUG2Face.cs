using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    public interface IColTHUG2Face
    {
        public ushort Flags { get; set; }
        public ushort TerrainType { get; set; }
        public ushort VertIndex1 { get; set; }
        public ushort VertIndex2 { get; set; }
        public ushort VertIndex3 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColTHUG2SmallFace : IColTHUG2Face
    {
        public ushort flags;
        public ushort terrainType;
        public byte vertIndex1;
        public byte vertIndex2;
        public byte vertIndex3;
        public byte padding;

        public ushort Flags { get { return flags; } set { flags = value; } }
        public ushort TerrainType { get { return terrainType; } set { terrainType = value; } }
        public ushort VertIndex1 { get { return vertIndex1; } set { vertIndex1 = (byte)value; } }
        public ushort VertIndex2 { get { return vertIndex2; } set { vertIndex2 = (byte)value; } }
        public ushort VertIndex3 { get { return vertIndex3; } set { vertIndex3 = (byte)value; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColTHUG2Face : IColTHUG2Face
    {
        public ushort flags;
        public ushort terrainType;
        public ushort vertIndex1;
        public ushort vertIndex2;
        public ushort vertIndex3;

        public ushort Flags { get { return flags; } set { flags = value; } }
        public ushort TerrainType { get { return terrainType; } set { terrainType = value; } }
        public ushort VertIndex1 { get { return vertIndex1; } set { vertIndex1 = value; } }
        public ushort VertIndex2 { get { return vertIndex2; } set { vertIndex2 = value; } }
        public ushort VertIndex3 { get { return vertIndex3; } set { vertIndex3 = value; } }
    }
}
