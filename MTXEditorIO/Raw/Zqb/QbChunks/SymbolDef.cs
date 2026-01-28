using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class SymbolDef : BaseChunk
    {
        public uint Value;
        public string Name = string.Empty;

        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadUInt32();
            Name = reader.ReadCString();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(Value);
            writer.WriteCString(Name);
        }
    }
}
