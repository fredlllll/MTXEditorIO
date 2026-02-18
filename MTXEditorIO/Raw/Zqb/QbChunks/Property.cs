using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Property : Empty
    {
        public Property() : base(QbChunkCode.Property) { }

        public override string ToString()
        {
            return ".";
        }
    }
}
