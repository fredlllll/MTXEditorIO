using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    public enum BSPSplitAxis : byte
    {
        X = 0,
        Y = 1,
        Z = 2,
        Leaf = 3,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColBSPNodeLeaf
    {
        public BSPSplitAxis type;
        public byte padding1;
        public ushort numFaces;
        public uint nodeFaceIndexOffset; //offset of the face list inside the bsp faces block
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColBSPNodeAxisSplit
    {
        public uint splitAxisAndPoint; //2 lsbs are split axis type, upper 30 are int(16*split point)
        public uint leftNodeOffset;

        public BSPSplitAxis SplitAxis { 
            get { return (BSPSplitAxis)(splitAxisAndPoint & 0b11); }
            set { splitAxisAndPoint = (splitAxisAndPoint & ~0b11u) | (uint)value; }
        }

        public uint SplitPoint
        {
            get { return splitAxisAndPoint >> 2; }
            set { splitAxisAndPoint = (splitAxisAndPoint & 0b11u) | (uint)(value<<2); }
        }
    }

    public class ColBSPNode : IReadableWriteable
    {
        public BSPSplitAxis type;
        public ColBSPNodeLeaf leaf;
        public ColBSPNodeAxisSplit axisSplit;

        public void ReadFrom(BinaryReader reader)
        {
            type = (BSPSplitAxis)(reader.ReadByte() & 0b11); //only 2 LSBs are important here
            reader.BaseStream.Position--;

            if (type == BSPSplitAxis.Leaf)
            {
                leaf = reader.ReadStruct<ColBSPNodeLeaf>();
            }
            else
            {
                axisSplit = reader.ReadStruct<ColBSPNodeAxisSplit>();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            if (type == BSPSplitAxis.Leaf)
            {
                writer.WriteStruct(leaf);
            }
            else
            {
                writer.WriteStruct(axisSplit);
            }
        }
    }
}
