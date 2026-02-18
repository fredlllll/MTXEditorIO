using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class RandomPermute : Random
    {
        public RandomPermute() : base(QbChunkCode.RandomPermute) { }

        public override string ToString()
        {
            return "randomPermute";
        }
    }
}
