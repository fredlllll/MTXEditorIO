using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class ElseIf : Empty
    {
        public ElseIf() : base(QbChunkCode.ElseIf) { }

        public override string ToString()
        {
            return "elseif";
        }
    }
}
