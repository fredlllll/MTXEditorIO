using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class NewLine2 : BaseChunk
    {
        public int Value;
        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadInt32();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
        }
    }
}
