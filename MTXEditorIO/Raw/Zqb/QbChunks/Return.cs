using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Return : Empty
    {
        public Return() : base(QbChunkCode.Return) { }

        public override string ToString()
        {
            return "return";
        }
    }
}
