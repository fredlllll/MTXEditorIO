using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public class TexPCImgLevel : IReadableWriteable
    {
        public uint size;
        public byte[] data = Array.Empty<byte>();

        public void ReadFrom(BinaryReader reader)
        {
            size = reader.ReadUInt32();
            data = reader.ReadBytes((int)size);
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(size);
            if (data != null)
            {
                writer.Write(data);
            }
        }
    }
}
