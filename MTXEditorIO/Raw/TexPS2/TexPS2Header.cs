using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexPS2Header
    {
        public uint id;
        public uint unknown1;
        public uint imageNum;
        public uint checksum;
        public InnerHeader innerHeader;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InnerHeader
    {
        public uint unknown1;
        public uint unknown2;
        public uint unknownNum;
        public uint unknown3;

        public override string ToString()
        {
            return $"unknown1: {unknown1}, unknown2: {unknown2}, unknownNum: {unknownNum}, unknown3: {unknown3}";
        }
    }
}
