using MTXEditorIO.Raw.TexPS2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Ps2TexToPng
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.GetFullPath(args[0]);

            string fileNameNoExt = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile));

            Console.WriteLine("Reading tex file: " + inputFile);
            TexPS2 tex = new TexPS2();
            using (var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                tex.ReadFrom(fs);
                Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");
            }

            for (int i = 0; i < tex.images.Length; ++i)
            {
                var img = tex.images[i];
                Console.WriteLine($"Saving image {i}");
                uint width = img.header.Width;
                uint height = img.header.Height;

                /*var paletteColors = ImageConversion.ParsePalette(img.palette, img.header.PaletteFormat);
                byte[] indices = img.imageData;
                if (img.header.Format == TextureFormat.Indexed4)
                {
                    //expand packed 4 bit indices to full 8 bit indices for easier processing
                    indices = ImageConversion._4To8BitIndices(img.imageData);
                }
                //^ this data would be needed if you were to actually write out a palettized image, but none of the libraries I tried seem to support that well
                */

                var imageColors = ImageConversion.GetImageColors(img);


                using Image<Rgba32> image = new Image<Rgba32>((int)width, (int)height);
                image.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                        for (int x = 0; x < pixelRow.Length; x++)
                        {
                            //flip vertically while copying
                            var color = imageColors[(height - y - 1) * (int)width + x];
                            pixelRow[x] = new Rgba32(color.r, color.g, color.b, color.a);
                        }
                    }
                });
                string outFile = $"{fileNameNoExt}_{i}.png";
                image.SaveAsPng(outFile);
            }
        }
    }
}
