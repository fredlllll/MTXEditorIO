using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class MathDiv : Empty
    {
        public MathDiv() : base(QbChunkCode.MathDiv) { }

        public override string ToString()
        {
            return "/";
        }
    }
}
