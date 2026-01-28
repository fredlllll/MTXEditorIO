using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Vector2 : BaseChunk
    {
        public Vec2 Value = new Vec2();

        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadStruct<Vec2>();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.WriteStruct(Value);
        }
    }
}
