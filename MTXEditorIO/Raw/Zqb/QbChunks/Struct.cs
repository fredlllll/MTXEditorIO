using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Struct : Empty
    {
        public Struct() : base(QbChunkCode.Struct) { }

        public override string ToString()
        {
            return "{";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
