using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Equals : Empty
    {
        public Equals() : base(QbChunkCode.Equals) { }

        public override string ToString()
        {
            return "=";
        }
    }
}
