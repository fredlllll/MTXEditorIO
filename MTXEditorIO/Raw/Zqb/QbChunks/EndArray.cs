using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndArray : Empty
    {
        public EndArray() : base(QbChunkCode.EndArray) { }

        public override string ToString()
        {
            return "]";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
