using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public class TexPCImgLevel
    {
        public uint size;
        public byte[]? data;

        public void ReadFrom(StructReader reader)
        {
            size = reader.ReadUInt32();
            data = reader.ReadBytes((int)size);
        }   
    }
}
