
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class NewLine2 : BaseChunk
    {
        public NewLine2() : base(QbChunkCode.NewLine2) { }

        public int Value;
        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadInt32();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"({Value})\r\n";
        }
    }
}
