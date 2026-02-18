using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Vector3 : BaseChunk
    {
        public Vector3() : base(QbChunkCode.Vector3) { }

        public Vec3 Value = new Vec3();

        public override void ReadFrom(BinaryReader reader)
        {
            Value = reader.ReadStruct<Vec3>();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.WriteStruct(Value);
        }

        public override string ToString()
        {
            return $"[{Value.x},{Value.y},{Value.z}]";
        }
    }
}
