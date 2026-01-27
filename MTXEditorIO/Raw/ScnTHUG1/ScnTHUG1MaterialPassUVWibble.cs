using MTXEditorIO.Raw.Shared;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassUVWibble
    {
        public Vec2 velocity;
        public Vec2 frequency;
        public Vec2 amplitude;
        public Vec2 phase;
    }
}