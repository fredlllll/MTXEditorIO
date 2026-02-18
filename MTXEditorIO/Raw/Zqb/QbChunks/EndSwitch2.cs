using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndSwitch2 : BaseChunk
    {
        public EndSwitch2() : base(QbChunkCode.EndSwitch2) { }

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
            return $"endswitch2({Value})";
        }
    }
}
