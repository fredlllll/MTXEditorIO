using MTXEditorIO.Raw.Shared;
using System;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [Flags]
    public enum ScnTHUG1SectorMeshFlags : uint
    {
        //no known values
        unknown = 0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1SectorMeshHeader
    {
        public Vec3 center;
        public float radius;
        public Vec3 bboxMin;
        public Vec3 bboxMax;
        public ScnTHUG1SectorMeshFlags flags;
        public uint materialChecksum; //this is apparently an id into the list of materials
        public uint numLodLevels;
    }
}