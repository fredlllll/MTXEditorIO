using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class LessEquals : Empty
    {
        public LessEquals() : base(QbChunkCode.LessEquals) { }

        public override string ToString()
        {
            return "<=";
        }
    }
}
