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

            using var sourceImage = Image.Load<Rgba32>(inputFile);

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
            img.header.SetWidth(width);
            img.header.SetHeight(height);
            if (args.UsePalette)
            {
                //only rgba8888 in palette, indices in data
                var palette = colors.Distinct().ToList(); //list so we get an IndexOf function we need later
                if (palette.Count > 256)
                {
                    throw new NotSupportedException("Image has more than 256 distinct colors, can not fit in palette");
                }
                img.palette = new RGBA8888Color[palette.Count];
                for (int i = 0; i < palette.Count; i++)
                {
                    var c = palette[i];
                    img.palette[i] = new RGBA8888Color() { r = c.R, g = c.G, b = c.B, a = c.A };
                }
                img.data = new byte[colors.Length];
                for (int i = 0; i < colors.Length; ++i)
                {
                    img.data[i] = (byte)palette.IndexOf(colors[i]);
                }
            }
            else
            {
                //raw rgba8888
                img.data = new byte[colors.Length * 4];
                for (int i = 0; i < colors.Length; i++)
                {
                    int datIndex = i * 4;
                    var c = colors[i];
                    img.data[datIndex] = c.B;
                    img.data[datIndex + 1] = c.G;
                    img.data[datIndex + 2] = c.R;
                    img.data[datIndex + 3] = c.A;
                }
            }

            using var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            img.WriteTo(fs);
        }
    }
}
