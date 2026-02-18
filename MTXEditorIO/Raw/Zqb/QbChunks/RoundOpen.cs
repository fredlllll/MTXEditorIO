using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class RoundOpen : Empty
    {
        public RoundOpen() : base(QbChunkCode.RoundOpen) { }

        public override string ToString()
        {
            return "(";
        }
    }
}
