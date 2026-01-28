using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class If2 : BaseChunk
    {
        public ushort Value;
        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadUInt16();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
        }
    }
}
