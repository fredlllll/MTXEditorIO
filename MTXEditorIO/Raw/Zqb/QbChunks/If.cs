using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class If : Empty
    {
        public If() : base(QbChunkCode.If) { }

        public override string ToString()
        {
            return "if";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
