using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class GreaterEquals : Empty
    {
        public GreaterEquals() : base(QbChunkCode.GreaterEquals) { }

        public override string ToString()
        {
            return ">=";
        }
    }
}
