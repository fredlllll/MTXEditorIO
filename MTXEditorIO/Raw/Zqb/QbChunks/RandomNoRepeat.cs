using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class RandomNoRepeat : Random
    {
        public RandomNoRepeat() : base(QbChunkCode.RandomNoRepeat) { }

        public override string ToString()
        {
            return "randomNoRepeat";
        }
    }
}
