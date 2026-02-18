using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Array : Empty
    {
        public Array() : base(QbChunkCode.Array) { }

        public override string ToString()
        {
            return "[";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
