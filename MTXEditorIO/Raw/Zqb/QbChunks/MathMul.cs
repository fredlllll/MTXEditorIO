using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class MathMul : Empty
    {
        public MathMul() : base(QbChunkCode.MathMul) { }

        public override string ToString()
        {
            return "*";
        }
    }
}
