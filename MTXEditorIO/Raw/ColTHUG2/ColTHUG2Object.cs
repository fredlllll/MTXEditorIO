using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTXEditorIO.Raw.ColTHUG2
{
    public class ColTHUG2Object
    {
        public ColTHUG2ObjectHeader header;
        public IColTHUG2Vertex[] vertices = Array.Empty<IColTHUG2Vertex>();
        public byte[] intensities = Array.Empty<byte>();
        public IColTHUG2Face[] faces = Array.Empty<IColTHUG2Face>();
    }
}
