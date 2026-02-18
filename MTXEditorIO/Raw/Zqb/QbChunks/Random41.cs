using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Random41 : Random
    {
        public Random41() : base(QbChunkCode.Random41) { }

        public override string ToString()
        {
            return "random41";
        }
    }
}
