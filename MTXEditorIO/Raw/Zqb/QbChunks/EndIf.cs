using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndIf : Empty
    {
        public EndIf() : base(QbChunkCode.EndIf) { }

        public override string ToString()
        {
            return "endif";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
