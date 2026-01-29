using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Img
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImgHeader
    {
        public uint version;
        public uint fileSize; //?
        public uint notImageWidth;
        public uint notImageHeight;
        public uint unknown1;
        public uint unknown2;
        public ushort imageWidth;
        public ushort imageHeight;
        public uint palSize;
    }
}
