using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Shared
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec4
    {
        public float x, y, z, w;

        public override string ToString()
        {
            return $"[{x}, {y}, {z}, {w}]";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3
    {
        public float x, y, z;

        public override string ToString()
        {
            return $"[{x}, {y}, {z}]";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2
    {
        public float x, y;

        public override string ToString()
        {
            return $"[{x}, {y}]";
        }
    }
}
