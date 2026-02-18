using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Break : Empty
    {
        public Break() : base(QbChunkCode.Break) { }

        public override string ToString()
        {
            return "break";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
