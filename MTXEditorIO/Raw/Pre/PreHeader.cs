using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Pre
{
    public enum PreVersion : ushort
    {
        pre0 = 0, //TODO: idk if this is just a placeholder, or if pre version 0 actually had 0x0000 in the id aswell
        pre1 = 1, //might not even exist, was missing in PreTool
        pre2 = 2,
        pre3 = 3,
        pre4 = 4,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PreHeader
    {
        public uint fileSize;
        public PreVersion version; //2,3,4
        public ushort id; //0xABCD
        public uint itemCount;
    }
}
