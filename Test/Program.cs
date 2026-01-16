using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using MTXEditorIO.Raw.TexPC;
using MTXEditorIO.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;
using System.Text.Unicode;

namespace Test
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "I:\\Projects\\MTX Mototrax PRO Modding\\texture that doesnt work\\travisZtex";
            string fileNameNoExt = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_dir");
            Directory.CreateDirectory(fileNameNoExt);

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


            TexPC tex = new TexPC();
            tex.ReadFrom(fs);

            for (int j = 0; j < tex.images.Length; ++j)
            {
                var img = tex.images[j];
                uint width = img.header.width;
                uint height = img.header.height;
                for (int i = 0; i < img.header.levels; ++i)
                {
                    var layer = img.levels[i];

                    var outData = layer.data;

                    BcDecoder decoder = new BcDecoder();
                    ColorRgba32[] colors;

                    switch (img.header.dxt)
                    {
                        case 1:
                            colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, BCnEncoder.Shared.CompressionFormat.Bc1);
                            break;
                        case 2: //a guess in the dark what this means, Bc1 without alpha also works on it, both produce a black image
                            colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, BCnEncoder.Shared.CompressionFormat.Bc1WithAlpha);
                            break;
                        default:
                            throw new Exception($"Unsupported DXT format: {img.header.dxt}");
                    }

                    Image<Rgba32> image = new Image<Rgba32>((int)width, (int)height);
                    image.ProcessPixelRows(accessor =>
                    {
                        for (int y = 0; y < accessor.Height; y++)
                        {
                            Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                            for (int x = 0; x < pixelRow.Length; x++)
                            {
                                var color = colors[y * (int)width + x];
                                pixelRow[x] = new Rgba32(color.r, color.g, color.b, color.a);
                            }
                        }
                    });
                    string outFile = $"{fileNameNoExt}/{j}_{i}_{img.header.width}x{img.header.height}.png";
                    image.SaveAsPng(outFile);


                    //outFile = $"{fileNameNoExt}/{j}_{i}_{img.header.width}x{img.header.height}.dxt{img.header.dxt}";
                    //using var outFs = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    //outFs.Write(outData, 0, layer.data.Length);
                    width /= 2;
                    height /= 2;
                }
            }

            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");
        }
    }
}
