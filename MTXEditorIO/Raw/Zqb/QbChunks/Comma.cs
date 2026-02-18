using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Comma : Empty
    {
        public Comma() : base(QbChunkCode.Comma) { }

        public override string ToString()
        {
            return ",";
        }
    }
}
