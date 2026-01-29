using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using BCnEncoder.Shared;
using CommunityToolkit.HighPerformance;
using MTXEditorIO.Raw.TexPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexEditor
{
    public static class ImageConversion
    {
        static readonly BcDecoder decoder = new BcDecoder();
        static readonly BcEncoder encoder = new BcEncoder();
        public static Image GetImageFromTexImg(TexPCImg img)
        {
            uint width = img.header.width;
            uint height = img.header.height;
            var layer = img.levels[0];

            ColorRgba32[] colors;
            switch (img.header.dxtVersion)
            {
                case 1:
                    colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, CompressionFormat.Bc1);
                    break;
                case 2: //a guess in the dark what this means, Bc1 without alpha also works on it, both produce a black image, DXT3/BC2 doesnt work
                    colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, CompressionFormat.Bc1WithAlpha);
                    break;
                case 5:
                    colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, CompressionFormat.Bc3);
                    break;
                default:
                    throw new Exception($"Unsupported DXT format: {img.header.dxtVersion}");
            }

            var image = new Bitmap((int)width, (int)height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = colors[y * (int)width + x];
                    image.SetPixel(x, y, System.Drawing.Color.FromArgb(color.a, color.r, color.g, color.b));
                }
            }
            return image;
        }

        public static TexPCImg GetTexImgFromImage(Bitmap img)
        {
            var texImg = new TexPCImg();
            var width = (uint)img.Width;
            var height = (uint)img.Height;
            texImg.header.width = width;
            texImg.header.height = height;
            texImg.header.texelDepth = 32;
            texImg.header.palDepth = 0;
            texImg.header.dxtVersion = 5; //dxt1 for when only 1 bit alpha needed, else dxt5 (but i dont want to check so i just use dxt5 all the time)
            texImg.header.palSize = 0;


            var colors = new ColorRgba32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var sourceColor = img.GetPixel(x, y);
                    colors[y * (int)width + x] = new ColorRgba32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A);
                }
            }

            var readOnlyMem = new ReadOnlyMemory2D<ColorRgba32>(colors, (int)height, (int)width);
            encoder.OutputOptions.Format = CompressionFormat.Bc3; //dxt5
            var texData = encoder.EncodeToRawBytes(readOnlyMem);

            texImg.header.levels = (uint)texData.Length;
            texImg.levels = new TexPCImgLevel[texData.Length];
            for (int j = 0; j < texData.Length; j++)
            {
                var level = texImg.levels[j] = new TexPCImgLevel();
                level.data = texData[j];
                level.size = (uint)level.data.Length;
            }
            return texImg;
        }

    }
}
