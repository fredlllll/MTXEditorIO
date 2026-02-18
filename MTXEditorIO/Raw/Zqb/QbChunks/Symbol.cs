using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Symbol : BaseChunk
    {
        public Symbol() : base(QbChunkCode.Symbol) { }

        public uint Value;
        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadUInt32();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"TODO:symbol:{Value}";
        }
    }
}
