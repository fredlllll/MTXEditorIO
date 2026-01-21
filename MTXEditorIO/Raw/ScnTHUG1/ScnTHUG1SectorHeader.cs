using MTXEditorIO.Raw.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [Flags]
    public enum SectorFlags : uint
    {
        HAS_TEXCOORDS = 0x01,
        HAS_VERTEX_COLORS = 0x02,
        HAS_VERTEX_NORMALS = 0x04,
        HAS_VERTEX_WEIGHTS = 0x10,
        HAS_VERTEX_COLOR_WIBBLES = 0x800,
        SHADOW_VOLUME = 0x200000,
        BILLBOARD_PRESENT = 0x800000,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1SectorHeader
    {
        public uint checksum;
        public int boneIndex;
        public SectorFlags flags;
        public uint numMeshes;
        public Vec3 boundingBoxMin;
        public Vec3 boundingBoxMax;
        public Vec4 boundingSphere;
    }
}
