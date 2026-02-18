using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Global : Empty
    {
        public Global() : base(QbChunkCode.Global) { }

        public override string ToString()
        {
            return "@";
        }
    }
}
