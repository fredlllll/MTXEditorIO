using BCnEncoder.Encoder;
using BCnEncoder.Shared;
using CommandLine;
using CommunityToolkit.HighPerformance;
using MTXEditorIO.Raw.TexPC;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ToTex
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineArgs>(args).WithParsed<CommandLineArgs>(Main);
        }

        static void Main(CommandLineArgs args)
        {
            string[] inputFiles = args.InputFiles.ToArray();

            TexPC tex = new TexPC();
            tex.header.imageNum = (uint)inputFiles.Length;
            tex.images = new TexPCImg[inputFiles.Length];

            var encoder = new BcEncoder();
            encoder.OutputOptions.Format = BCnEncoder.Shared.CompressionFormat.Bc1;

            for (int i = 0; i < inputFiles.Length; i++)
            {
                using var sourceImage = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(inputFiles[i]);

                int width = sourceImage.Width;
                int height = sourceImage.Height;
                var colors = new ColorRgba32[width * height];
                sourceImage.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                        for (int x = 0; x < pixelRow.Length; x++)
                        {
                            var sourceColor = pixelRow[x];
                            colors[y * (int)width + x] = new ColorRgba32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A);
                        }
                    }
                });

                var readOnlyMem = new ReadOnlyMemory2D<ColorRgba32>(colors, height, width);
                var texData = encoder.EncodeToRawBytes(readOnlyMem); //creates all mipmaps encoded in dxt1

                var texImage = tex.images[i] = new TexPCImg();
                texImage.header.width = (uint)width;
                texImage.header.height = (uint)height;
                texImage.header.dxt = 1;
                texImage.header.levels = (uint)texData.Length;
                texImage.levels = new TexPCImgLevel[texData.Length];
                for(int j = 0; j< texData.Length; j++)
                {
                    var level = texImage.levels[j] = new TexPCImgLevel();
                    level.data = texData[j];
                    level.size = (uint)level.data.Length;
                }
            }

            using var fs = new FileStream(args.OutputFile, FileMode.Create, FileAccess.Write);
            tex.WriteTo(fs);
        }
    }
}
