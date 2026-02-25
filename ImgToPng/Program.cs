using CommandLine;
using MTXEditorIO.Raw.Img;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Net;
using System.Text;

namespace ImgToPng
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
            string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + ".png");

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var img = new Img();
            img.ReadFrom(fs);
            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");
            ushort width = img.header.imageWidth;
            ushort height = img.header.imageHeight;
            Rgba32[] colors = new Rgba32[width * height];

            if (img.palette.Length > 0)
            {
                var pal = img.palette;
                Rgba32[] palette = new Rgba32[img.palette.Length / 4];
                for (int i = 0; i < palette.Length; i++)
                {
                    var palDataIndex = i * 4;
                    palette[i] = new Rgba32(pal[palDataIndex], pal[palDataIndex + 1], pal[palDataIndex + 2], pal[palDataIndex + 3]);
                }
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = palette[img.data[i]];
                }
            }
            else
            {
                var dat = img.data;
                for (int i = 0; i < colors.Length; i++)
                {
                    var dataIndex = i * 4;
                    colors[i] = new Rgba32(dat[dataIndex], dat[dataIndex + 1], dat[dataIndex + 2], dat[dataIndex + 3]);
                }
            }

            using Image<Rgba32> image = new Image<Rgba32>(width, height);
            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        //flip vertically while copying
                        int index = (height - y - 1) * width + x;
                        pixelRow[x] = colors[index];
                    }
                }
            });
            image.SaveAsPng(outputFile);
        }
    }
}
