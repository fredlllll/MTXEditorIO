using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Terminator : Empty
    {
        public Terminator() : base(QbChunkCode.Terminator) { }

        public override string ToString()
        {
            return "HALT";
        }
    }
}
