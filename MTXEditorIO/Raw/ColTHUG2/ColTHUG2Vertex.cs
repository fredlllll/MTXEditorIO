using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    public interface IColTHUG2Vertex
    {
        Vec4 GetPos(ColTHUG2Object obj);
    }

    public struct ColTHUG2FixedVertex : IColTHUG2Vertex
    {
        public ushort x, y, z;

        public Vec4 GetPos(ColTHUG2Object obj)
        {
            return new Vec4()
            {
                x = obj.header.boundingBoxMin.x + x * 0.0625f,
                y = obj.header.boundingBoxMin.y + y * 0.0625f,
                z = obj.header.boundingBoxMin.z + z * 0.0625f
            };
        }
    }

    public struct ColTHUG2Vertex : IColTHUG2Vertex
    {
        public float x, y, z;

        public Vec4 GetPos(ColTHUG2Object obj)
        {
            return new Vec4()
            {
                x = x,
                y = y,
                z = z,
            };
        }
    }
}
