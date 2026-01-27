using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialFixedHeader
    {
        public uint checksum;
        public uint materialNameChecksum;
        public uint materialPasses;
        public uint alphaCutoff; // only LSB is used, perhaps this field isnt correct
        public OneByteBoolean sorted;
        public float drawOrder;
        public OneByteBoolean singleSided;
        public OneByteBoolean noBackfaceCulling;
        public int zBias;
        public OneByteBoolean grassify;

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}
