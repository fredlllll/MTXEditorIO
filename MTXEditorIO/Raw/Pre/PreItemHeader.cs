using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.Pre
{
    public interface IPreItemHeader
    {
        public uint InflatedSize { get; set; }
        public uint DeflatedSize { get; set; }
        public uint FileNameLength { get; set; }
        public uint FileNameCrc { get; set; }
        public bool IsCompressed => (DeflatedSize != 0) && (DeflatedSize < InflatedSize);
    }

    public struct PreItemHeader2 : IPreItemHeader
    {
        public uint inflatedSize;
        public uint deflatedSize;
        public uint fileNameLength;

        public uint InflatedSize { get { return inflatedSize; } set { inflatedSize = value; } }
        public uint DeflatedSize { get { return deflatedSize; } set { deflatedSize = value; } }
        public uint FileNameLength { get { return fileNameLength; } set { fileNameLength = value; } }
        public uint FileNameCrc { get { return 0; } set { } }
    }

    public struct PreItemHeader3_4 : IPreItemHeader
    {
        public uint inflatedSize;
        public uint deflatedSize;
        public uint fileNameLength;
        public uint fileNameCrc;

        public uint InflatedSize { get { return inflatedSize; } set { inflatedSize = value; } }
        public uint DeflatedSize { get { return deflatedSize; } set { deflatedSize = value; } }
        public uint FileNameLength { get { return fileNameLength; } set { fileNameLength = value; } }
        public uint FileNameCrc { get { return fileNameCrc; } set { fileNameCrc = value; } }
    }
}
