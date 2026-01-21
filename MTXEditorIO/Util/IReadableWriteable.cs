using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Util
{
    public interface IReadableWriteable
    {
        void ReadFrom(BinaryReader reader);
        void WriteTo(BinaryWriter writer);
    }

    public interface IReadableWriteableFromStream
    {
        void ReadFrom(Stream stream);
        void WriteTo(Stream stream);
    }
}
