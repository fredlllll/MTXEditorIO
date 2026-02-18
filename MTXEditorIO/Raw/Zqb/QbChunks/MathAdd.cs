using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class MathAdd : Empty
    {
        public MathAdd() : base(QbChunkCode.MathAdd) { }

        public override string ToString()
        {
            return "+";
        }
    }
}
