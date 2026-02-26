using CommandLine;
using MTXEditorIO.Raw.Img;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

            int width = (int)img.header.imageDataWidth;
            int height = (int)img.header.imageDataHeight;
            Rgba32[] colors = new Rgba32[width * height];

            Console.WriteLine("Image format: " + img.header.pixelFormat);

            switch (img.header.pixelFormat)
            {
                case ImgPixelFormat.BGRA8888:
                    var dat = img.data;
                    for (int i = 0; i < colors.Length; i++)
                    {
                        var dataIndex = i * 4;
                        colors[i] = new Rgba32(dat[dataIndex+2], dat[dataIndex + 1], dat[dataIndex], dat[dataIndex + 3]);
                    }
                    break;
                case ImgPixelFormat.Indexed8:
                    var pal = img.palette;
                    Rgba32[] palette = new Rgba32[img.palette.Length];
                    for (int i = 0; i < palette.Length; i++)
                    {
                        var palColor = img.palette[i];

                        palette[i] = new Rgba32(palColor.r, palColor.g, palColor.b, palColor.a);
                    }
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = palette[img.data[i]];
                    }
                    break;
                default:
                    throw new NotImplementedException("unsupported format: " + img.header.pixelFormat);
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
