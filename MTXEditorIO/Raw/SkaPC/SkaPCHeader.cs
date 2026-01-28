using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.SkaPC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkaPCHeader
    {
        public uint id; //1
        public ushort unknown1;
        public byte unknown2;
        public byte unknown3;
        public float duration;
        public uint numShortPairs;
        public uint numFrames;
        public uint numUnknown1;
        public uint numUnknown2;
        public uint numUnknown3;
        public uint numUnknown4;
    }
}
