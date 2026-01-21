using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColTHUG2Header
    {
        public uint version; //should be 9
        public uint numObjects;
        public uint totalVerts;
        public uint totalLargeFaces;

        public uint totalSmallFaces;
        public uint totalLargeVerts;
        public uint totalSmallVerts;
        public uint unknown;

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}
