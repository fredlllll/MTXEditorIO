using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Or : Empty
    {
        public Or() : base(QbChunkCode.Or) { }

        public override string ToString()
        {
            return "||";
        }
    }
}
