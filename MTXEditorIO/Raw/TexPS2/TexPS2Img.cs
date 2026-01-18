using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public class TexPS2Img : IReadableWriteable
    {
        public readonly bool isFirstImage;

        public ITexPS2ImgHeader header = new TexPS2ImgHeader();
        public byte[] palette = Array.Empty<byte>();
        public byte[] imageData = Array.Empty<byte>();
        public byte[][] mipMaps = Array.Empty<byte[]>();

        public TexPS2Img(bool isFirstImage)
        {
            this.isFirstImage = isFirstImage;
        }

        public void ReadFrom(StructReader reader)
        {
            uint id = reader.ReadUInt32();
            if (!isFirstImage && id != 0)
            {
                //the first uint of all image headers has so far been 0
                //the CR125 file has a random structure in the file (appearing after image 27):
                // F5 1D 19 16 00 00 00 00 00 80 BB 44 02 00 00 00
                //structure doesnt begin with 0, so at least that sets it apart from image headers.
                //skip these 16 bytes (12 cause we already read 4)
                reader.ReadBytes(12);
            }
            else
            {
                //unread
                reader.BaseStream.Position -= 4;
            }

            //for some reason the first header is formatted differently
            long headerPos = reader.BaseStream.Position;
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
                throw new Exception($"Image too large: {width}*{height}, header at {headerPos}");
            }
            int paletteItemCount = 0;
            int paletteItemSize = 0;
            int dataSize = 0;
            switch (header.Format)
            {
                case PixelFormat.Indexed4:
                    paletteItemCount = 16; // 16 colors
                    dataSize = width * height / 2; // 4 bits per pixel
                    break;
                case PixelFormat.Indexed8:
                    paletteItemCount = 256; // 256 colors
                    dataSize = width * height; // 8 bits per pixel
                    break;
                case PixelFormat.ABGR1555:
                    dataSize = width * height * 2; //16 bit per pixel
                    break;
                case PixelFormat.RGBA8888:
                    dataSize = width * height * 4; //32 bits per pixel
                    break;
                default:
                    throw new Exception($"Unsupported texture format: {header.Format}, header at {headerPos}");
            }
            switch (header.PaletteFormat)
            {
                case PixelFormat.ABGR1555:
                    paletteItemSize = 2;
                    break;
                case PixelFormat.RGBA8888:
                    paletteItemSize = 4;
                    break;
                default:
                    throw new Exception($"Unsupported palette format: {header.PaletteFormat}, header at {headerPos}");
            }
            palette = reader.ReadBytes(paletteItemCount * paletteItemSize);
            imageData = reader.ReadBytes(dataSize);
            mipMaps = new byte[header.MipMapLevels][];
            for (int i = 0; i < header.MipMapLevels; ++i)
            {
                dataSize /= 4;
                mipMaps[i] = reader.ReadBytes(dataSize);
            }
        }

        public void WriteTo(StructWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
