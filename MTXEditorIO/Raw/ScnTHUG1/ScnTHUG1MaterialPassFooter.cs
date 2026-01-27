using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassFooter
    {
        public uint mmag;
        public uint mmin;
        public float lodBias;
        public float l;
    }
}