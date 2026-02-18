using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class GlobalAll : Empty
    {
        public GlobalAll() : base(QbChunkCode.GlobalAll) { }

        public override string ToString()
        {
            return "<...>";
        }
    }
}
