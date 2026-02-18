using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Empty : BaseChunk
    {
        public Empty(QbChunkCode chunkCode) : base(chunkCode)
        {
        }

        public override void ReadFrom(BinaryReader reader)
        {

        }

        public override void WriteTo(BinaryWriter writer)
        {

        }
    }
}
