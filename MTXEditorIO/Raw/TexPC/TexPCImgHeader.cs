using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    [StructLayout(LayoutKind.Sequential)]
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

        public override string ToString()
        {
            return $"TexPCImgHeader: sum={sum}, width={width}, height={height}, levels={levels}, dxt={dxt}, unknown1={unknown1}, unknown2={unknown2}, unknown3={unknown3}";
        }
    }
}
