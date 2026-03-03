using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public class TexPS2Img : IReadableWriteable
    {
        public readonly bool isFirstImage;

        public TexPS2ImgHeader header = new TexPS2ImgHeader();
        public byte[] palette = Array.Empty<byte>();
        public byte[] imageData = Array.Empty<byte>();
        public byte[][] mipMaps = Array.Empty<byte[]>();

        public TexPS2Img(bool isFirstImage)
        {
            this.isFirstImage = isFirstImage;
        }

        public void ReadFrom(BinaryReader reader)
        {
            //for some reason the first header is formatted differently
            long headerPos = reader.BaseStream.Position;
            header.ReadFrom(reader);
            Console.WriteLine(header);

            int width = 1 << (int)header.widthPowOfTwo;
            int height = 1 << (int)header.heightPowOfTwo;
            if (width > 2048 || height > 2048) //dont think the game can do anything above 512, so in case we read garbage exit early
            {
                throw new Exception($"Image too large: {width}*{height}, header at {headerPos}");
            }
            int paletteItemCount = 0;
            int paletteItemSize = 0;
            int dataSize = 0;
            switch (header.format)
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
                    throw new Exception($"Unsupported texture format: {header.format}, header at {headerPos}");
            }
            switch (header.paletteFormat)
            {
                case PixelFormat.ABGR1555:
                    paletteItemSize = 2;
                    break;
                case PixelFormat.RGBA8888:
                    paletteItemSize = 4;
                    break;
                default:
                    throw new Exception($"Unsupported palette format: {header.paletteFormat}, header at {headerPos}");
            }
            if (header.noDataIfNotZero == 0)
            {
                reader.BaseStream.Position = (reader.BaseStream.Position + 15) & ~15;//T align stream to next 16 bytes
                palette = reader.ReadBytes(paletteItemCount * paletteItemSize);
                imageData = reader.ReadBytes(dataSize);
                mipMaps = new byte[header.mipMapLevels][];
                for (int i = 0; i < header.mipMapLevels; ++i)
                {
                    dataSize /= 4;
                    mipMaps[i] = reader.ReadBytes(dataSize);
                }
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
