using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TexPCHeader
    {
        public uint id;
        public uint imageNum;
    }
}
