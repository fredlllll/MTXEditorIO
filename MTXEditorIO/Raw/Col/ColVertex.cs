using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    public interface IColVertex
    {
        Vec3 GetPos(ColObject obj);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColSmallVertex : IColVertex
    {
        public ushort x, y, z;

        public Vec3 GetPos(ColObject obj)
        {
            return new Vec3()
            {
                x = obj.header.boundingBoxMin.x + x * 0.0625f,
                y = obj.header.boundingBoxMin.y + y * 0.0625f,
                z = obj.header.boundingBoxMin.z + z * 0.0625f
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColVertex : IColVertex
    {
        public float x, y, z;

        public Vec3 GetPos(ColObject obj)
        {
            return new Vec3()
            {
                x = x,
                y = y,
                z = z,
            };
        }
    }
}
