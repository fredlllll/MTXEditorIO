using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Repeat : Empty
    {
        public Repeat() : base(QbChunkCode.Repeat) { }

        public override string ToString()
        {
            return "repeat";
        }
    }
}
