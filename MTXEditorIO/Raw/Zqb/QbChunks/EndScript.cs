using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndScript : Empty
    {
        public EndScript() : base(QbChunkCode.EndScript) { }

        public override string ToString()
        {
            return "endscript";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
