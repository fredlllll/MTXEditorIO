using MTXEditorIO.Util;
using System;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.GeomPS2
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GeomPS2Header
    {
        public uint unknown0; //0x00: always 0x10 (16)
        public uint unknown4; //0x04: always 0x10 (16)
        public uint unknown8; //0x08: 0
        public uint unknownC; //0x0C: 0
        public uint recordTableOffset; //0x10: offset of the 80-byte record table at the end of the file
        public uint unknown14; //0x14: 0
        public uint unknown18; //0x18: 0
        public uint unknown1C; //0x1C: 0

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}