using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Less : Empty
    {
        public Less() : base(QbChunkCode.Less) { }

        public override string ToString()
        {
            return "<";
        }
    }
}
