using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Param : String
    {
        public Param() : base(QbChunkCode.Param) { }

        public override string ToString()
        {
            return $"'{Value}'";
        }
    }
}
