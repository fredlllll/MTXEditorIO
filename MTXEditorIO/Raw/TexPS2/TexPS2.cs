using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public class TexPS2
    {
        public TexPS2Header header;
        public TexPS2Img[] images = Array.Empty<TexPS2Img>();

        public void ReadFrom(Stream stream)
        {
            using var reader = new StructReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<TexPS2Header>();
            images = new TexPS2Img[header.imageNum];
            for (int i = 0; i < header.imageNum; i++)
            {
                //Console.WriteLine($"Reading Image {i}");
                var img = images[i] = new TexPS2Img(i == 0);
                img.ReadFrom(reader);
            }
        }
    }
}
