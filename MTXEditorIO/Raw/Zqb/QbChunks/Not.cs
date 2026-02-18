using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Not : Empty
    {
        public Not() : base(QbChunkCode.Not) { }

        public override string ToString()
        {
            return "not";
        }
    }
}
