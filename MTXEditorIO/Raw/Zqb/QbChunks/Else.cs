using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Else : Empty
    {
        public Else() : base(QbChunkCode.Else) { }

        public override string ToString()
        {
            return "else";
        }
    }
}
