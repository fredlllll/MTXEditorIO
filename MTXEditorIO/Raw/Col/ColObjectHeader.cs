using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColObjectHeader
    {
        public uint checksum;
        public ushort flags;
        public ushort numVerts;
        public ushort numFaces;
        public OneByteBoolean useSmallFaces; //bool
        public OneByteBoolean useSmallVerts; //bool
        public uint firstFaceOffset;
        public Vec4 boundingBoxMin;
        public Vec4 boundingBoxMax;
        public uint firstVertOffset;
        public uint bspTreeHeadOffset;
        public uint intensityOffset;
        public uint padding;

        public override string ToString()
        {
            return Output.ToString(this, "\n");
        }
    }
}
