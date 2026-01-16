using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using CommandLine;
using MTXEditorIO.Raw.TexPC;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;

namespace TexToPng
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineArgs>(args).WithParsed<CommandLineArgs>(Main);
        }

        static void Main(CommandLineArgs args)
        {
            string inputFile = args.InputFile;
            string fileNameNoExt = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_dir");
            Directory.CreateDirectory(fileNameNoExt);

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            BcDecoder decoder = new BcDecoder();
            TexPC tex = new TexPC();
            tex.ReadFrom(fs);
            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");

            for (int j = 0; j < tex.images.Length; ++j)
            {
                var img = tex.images[j];
                uint width = img.header.width;
                uint height = img.header.height;
                for (int i = 0; i < img.header.levels; ++i)
                {
                    var layer = img.levels[i];
                    
                    ColorRgba32[] colors;
                    switch (img.header.dxt)
                    {
                        case 1:
                            colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, CompressionFormat.Bc1);
                            break;
                        case 2: //a guess in the dark what this means, Bc1 without alpha also works on it, both produce a black image
                            colors = decoder.DecodeRaw(layer.data, (int)width, (int)height, CompressionFormat.Bc1WithAlpha);
                            break;
                        default:
                            throw new Exception($"Unsupported DXT format: {img.header.dxt}");
                    }

                    using Image<Rgba32> image = new Image<Rgba32>((int)width, (int)height);
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

                    width /= 2; //each subsequent mipmap is half the size
                    height /= 2;
                    if (!args.DumpMips)
                    {
                        break; //exit after first iteration if we shouldnt dump the mip maps
                    }
                }
            }


        }
    }
}
