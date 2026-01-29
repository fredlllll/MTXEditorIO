using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexPCImgHeader
    {
        public uint checksum;
        public uint width;
        public uint height;
        public uint levels;
        public uint texelDepth;
        public uint palDepth;
        public uint dxtVersion;
        public uint palSize;

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}
