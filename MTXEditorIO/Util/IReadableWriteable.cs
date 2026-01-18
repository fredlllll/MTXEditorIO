using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Util
{
    public interface IReadableWriteable
    {
        void ReadFrom(StructReader reader);
        void WriteTo(StructWriter writer);
    }

    public interface IReadableWriteableFromStream
    {
        void ReadFrom(Stream stream);
        void WriteTo(Stream stream);
    }
}
