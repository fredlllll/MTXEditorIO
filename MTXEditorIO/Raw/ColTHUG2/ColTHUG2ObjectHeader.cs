using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColTHUG2ObjectHeader
    {
        public uint checksum;
        public ushort flags;
        public ushort numVerts;
        public ushort numFaces;
        public byte useSmallFaces; //bool
        public byte useFixedVerts; //bool
        public uint firstFaceOffset; //unsure, is 0 for the first one. perhaps pointer relative to after header pos?
        public Vec4 boundingBoxMin;
        public Vec4 boundingBoxMax;
        public uint firstVertOffset;
        public uint bspTreeHeadOffset;
        public uint intensityOffset;
        public uint padding;


        /*public uint type;
        public uint checksum;
        public ushort x, y, z, w;
        public float x1, y1, z1, w1;
        public float x2, y2, z2, w2;
        public uint index1, index2, index3, index4;*/

        public override string ToString()
        {
            return Output.ToString(this,"\n");
        }
    }
}
