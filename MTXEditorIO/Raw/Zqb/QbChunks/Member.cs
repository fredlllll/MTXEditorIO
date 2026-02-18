using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Member : Empty
    {
        public Member() : base(QbChunkCode.Member) { }

        public override string ToString()
        {
            return ":";
        }
    }
}
