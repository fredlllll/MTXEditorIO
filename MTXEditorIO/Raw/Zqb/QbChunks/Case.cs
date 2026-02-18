using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Case : Empty
    {
        public Case() : base(QbChunkCode.Case) { }

        public override string ToString()
        {
            return "case";
        }
    }
}
