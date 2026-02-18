using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class EndStruct : Empty
    {
        public EndStruct() : base(QbChunkCode.EndStruct) { }

        public override string ToString()
        {
            return "}";
        }

        public override IndentationModifier IndentationModifier => IndentationModifier.ThisUnindent;
    }
}
