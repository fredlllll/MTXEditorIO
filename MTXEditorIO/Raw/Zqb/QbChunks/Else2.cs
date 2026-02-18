using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Else2 : BaseChunk
    {
        public Else2() : base(QbChunkCode.Else2) { }

        public ushort Value;
        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadUInt16();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"else2({Value})";
        }
    }
}
