using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class And : Empty
    {
        public And() : base(QbChunkCode.And) { }

        public override string ToString()
        {
            return "&&";
        }
    }
}
