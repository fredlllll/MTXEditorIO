using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class String : BaseChunk
    {
        public string Value = string.Empty;

        public String() : base(QbChunkCode.String) { }
        protected String(QbChunkCode code) : base(code) { }

        public override void ReadFrom(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            Value = StringUtils.GetStringSafe(reader.ReadBytes(length));
        }

        public override void WriteTo(BinaryWriter writer)
        {
            var bytes = StringUtils.GetBytes(Value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        public override string ToString()
        {
            return $"\"{Value}\"";
        }
    }
}
