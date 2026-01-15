using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public class TexPS2Img
    {
        public readonly bool isFirstImage;

        public ITexPS2ImgHeader header = new TexPS2ImgHeader();
        public byte[] palette = Array.Empty<byte>();
        public byte[] imageData = Array.Empty<byte>();

        public TexPS2Img(bool isFirstImage)
        {
            this.isFirstImage = isFirstImage;
        }

        public void ReadFrom(StructReader reader)
        {
            //for some reason the first header is formatted differently
            if (isFirstImage)
            {
                header = reader.ReadStruct<TexPS2FirstImgHeader>();
            }
            else
            {
                header = reader.ReadStruct<TexPS2ImgHeader>();
            }
            Console.WriteLine(header);

            int width = 1 << (int)header.WidthPowOfTwo;
            int height = 1 << (int)header.HeightPowOfTwo;
            if (width > 512 || height > 512) //dont think the game can do anything above 512, so in case we read garbage exit early
            {
                throw new Exception($"Image too large: {width}*{height}");
            }
            int paletteItemCount = 0;
            int paletteItemSize = 0;
            int dataSize = 0;
            switch (header.Format)
            {
                case TextureFormat.Indexed4:
                    paletteItemCount = 16; // 16 colors
                    dataSize = width * height / 2; // 4 bits per pixel
                    break;
                case TextureFormat.Indexed8:
                    paletteItemCount = 256; // 256 colors
                    dataSize = width * height; // 8 bits per pixel
                    break;
                default:
                    throw new Exception("Unsupported texture format: " + header.Format);
            }
            switch (header.PaletteFormat)
            {
                case PaletteFormat.ABGR1555:
                    paletteItemSize = 2;
                    break;
                case PaletteFormat.RGBA8888:
                    paletteItemSize = 4;
                    break;
                default:
                    throw new Exception("Unsupported palette format: " + header.PaletteFormat);
            }
            palette = reader.ReadBytes(paletteItemCount * paletteItemSize);
            imageData = reader.ReadBytes(dataSize);
        }
    }
}
