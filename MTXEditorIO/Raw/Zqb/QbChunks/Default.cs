using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Default : Empty
    {
        public Default() : base(QbChunkCode.Default) { }

        public override string ToString()
        {
            return "default";
        }
    }
}
