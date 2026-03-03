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
    }
}
