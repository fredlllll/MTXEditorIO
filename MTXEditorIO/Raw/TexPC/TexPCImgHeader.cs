using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public struct TexPCImgHeader
    {
        public uint sum;
        public uint width;
        public uint height;
        public uint levels;
        public uint unknown1;
        public uint unknown2;
        public uint dxt;
        public uint unknown3;
    }
}
