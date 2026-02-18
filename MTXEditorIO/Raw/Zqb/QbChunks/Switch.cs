using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Switch : Empty
    {
        public Switch() : base(QbChunkCode.Switch) { }

        public override string ToString()
        {
            return "switch";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.NextIndent;
    }
}
