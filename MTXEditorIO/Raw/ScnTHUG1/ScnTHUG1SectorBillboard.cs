using MTXEditorIO.Raw.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public enum BillboardType : uint
    {
        Screen = 1,
        Axis = 2,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1SectorBillboard
    {
        public BillboardType billboardType;
        public Vec3 origin;
        public Vec3 pivotPos;
        public Vec3 pivotAxis;
    }
}
