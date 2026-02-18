using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class MathSub : Empty
    {
        public MathSub() : base(QbChunkCode.MathSub) { }

        public override string ToString()
        {
            return "-";
        }
    }
}
