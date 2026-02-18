using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndSwitch : Empty
    {
        public EndSwitch() : base(QbChunkCode.EndSwitch) { }

        public override string ToString()
        {
            return "endswitch";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
