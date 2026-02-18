using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class RoundClose : Empty
    {
        public RoundClose() : base(QbChunkCode.RoundClose) { }

        public override string ToString()
        {
            return ")";
        }
    }
}
