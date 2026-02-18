using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Greater : Empty
    {
        public Greater() : base(QbChunkCode.Greater) { }

        public override string ToString()
        {
            return ">";
        }
    }
}
