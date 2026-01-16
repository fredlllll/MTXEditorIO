using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public class TexPC
    {
        public TexPCHeader header;
        public TexPCImg[] images = Array.Empty<TexPCImg>();

        public void ReadFrom(Stream stream)
        {
            using var reader = new StructReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<TexPCHeader>();
            images = new TexPCImg[header.imageNum];
            for (int i = 0; i < header.imageNum; i++)
            {
                var img = images[i] = new TexPCImg();
                img.ReadFrom(reader);
            }
        }

        public void WriteTo(Stream stream)
        {
            using var writer = new StructWriter(stream, Encoding.ASCII, true);
            writer.WriteStruct(header);
            for (int i = 0; i < header.imageNum; i++)
            {
                images[i].WriteTo(writer);
            }
        }
    }
}
