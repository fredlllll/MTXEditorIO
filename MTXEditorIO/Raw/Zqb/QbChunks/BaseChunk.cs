using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public abstract class BaseChunk : IReadableWriteable
    {
        public int OffsetInFile = -1;
        public readonly QbChunkCode chunkCode;
        public BaseChunk(QbChunkCode chunkCode)
        {
            this.chunkCode = chunkCode;
        }

        public abstract void ReadFrom(BinaryReader reader);
        public abstract void WriteTo(BinaryWriter writer);

        public virtual IndentationModifier IndentationModifier { get; } = IndentationModifier.None;
    }
}
