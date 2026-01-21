using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1SectorVertexBoneIndices
    {
        public ushort boneIndex1;
        public ushort boneIndex2;
        public ushort boneIndex3;
        public ushort boneIndex4;
    }
}
