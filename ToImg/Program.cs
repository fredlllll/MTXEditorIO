using CommandLine;
using MTXEditorIO.Raw.Img;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ToImg
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineArgs>(args).WithParsed<CommandLineArgs>(Main);
        }

        public static void Main(CommandLineArgs args)
        {
            string inputFile = Path.GetFullPath(args.InputFile);
            string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + ".img");

            if (args.UsePalette)
            {
                throw new NotImplementedException("palette creation is not implemented yet");
            }

            using var sourceImage = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(inputFile);

            int width = sourceImage.Width;
            int height = sourceImage.Height;
            Rgba32[] colors = new Rgba32[width * height];
            sourceImage.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        var sourceColor = pixelRow[x];
                        // Flip vertically while copying
                        colors[(height - y - 1) * width + x] = sourceColor;
                    }
                }
            });


            var img = new Img();
            img.header.version = 2;
            img.header.imageWidth = (ushort)width;
            img.header.imageHeight = (ushort)height;
            img.data = new byte[colors.Length * 4];
            for (int i = 0; i < colors.Length; i++)
            {
                int datIndex = i * 4;
                var c = colors[i];
                img.data[datIndex] = c.R;
                img.data[datIndex + 1] = c.G;
                img.data[datIndex + 2] = c.B;
                img.data[datIndex + 3] = c.A;
            }

            using var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            img.WriteTo(fs);
        }
    }
}
