using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class NewLine1 : Empty
    {
        public NewLine1() : base(QbChunkCode.NewLine1) { }

        public override string ToString()
        {
            return "\r\n";
        }
    }
}
