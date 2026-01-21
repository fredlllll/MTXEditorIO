using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexPCHeader
    {
        /// <summary>
        /// supposed to be 1
        /// </summary>
        public uint id;
        /// <summary>
        /// number of images in this tex file
        /// </summary>
        public uint imageNum;
    }
}
