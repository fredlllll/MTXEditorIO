using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassTextureAnimatesKeyframe
    {
        public uint time;
        public uint image; //probably a pointer into a list or dictionary or tex file?
    }
}