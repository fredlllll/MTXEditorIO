using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class RandomRange : Empty
    {
        public RandomRange() : base(QbChunkCode.RandomRange) { }

        public override string ToString()
        {
            return "randomRange";
        }
    }
}
