using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Script : Empty
    {
        public Script() : base(QbChunkCode.Script) { }

        public override string ToString()
        {
            return "script";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
