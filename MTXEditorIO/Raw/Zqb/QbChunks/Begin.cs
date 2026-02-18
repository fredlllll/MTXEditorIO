using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Begin : Empty
    {
        public Begin() : base(QbChunkCode.Begin) { }
        public override string ToString()
        {
            return "begin";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
