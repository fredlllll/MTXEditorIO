using MTXEditorIO.Raw.TexPS2;
using MTXEditorIO.Util;
using System.Text.Unicode;

namespace Test
{
    internal static class Program
    {
        static byte ExpandTo8(byte fieldValue, int fieldSize)
        {
            int max = (1 << fieldSize) - 1;
            return (byte)((fieldValue * 255 + (max >> 1)) / max);
        }

        public static byte ExtractField(ushort value, int bitOffset, int fieldSize)
        {
            if (fieldSize > 8)
            {
                throw new Exception("field size cannot be larger than 8 bits");
            }
            if (bitOffset + fieldSize > 16)
            {
                throw new Exception("bit offset + field size cannot be larger than 16 bits");
            }
            int mask = (1 << fieldSize) - 1;
            // with 3 we get 0b0000000000000111
            //have to shift value over so the field we want lines up with the mask
            //e.g. bitoffset is 11 , so our field is at 0b0000000000011100
            int shifted = value >> (16 - bitOffset - fieldSize);
            shifted &= mask;
            return ExpandTo8((byte)shifted, fieldSize);
        }

        public static (byte r, byte g, byte b, byte a)[] ParsePalette(ushort[] palette, string pixelFormat)
        {
            string colorOrder = pixelFormat.Substring(0, 4).ToUpper();
            string fieldSizes = pixelFormat.Substring(4);

            (byte r, byte g, byte b, byte a)[] result = new (byte r, byte g, byte b, byte a)[palette.Length];
            for (int i = 0; i < palette.Length; ++i)
            {
                var paletteEntry = palette[i];

                int bitOffset = 0;
                byte r = 0, g = 0, b = 0, a = 255;
                for (int colorIndex = 0; colorIndex < 4; ++colorIndex)
                {
                    int fieldSize = int.Parse(fieldSizes[colorIndex].ToString());
                    if (fieldSize == 0)
                    {
                        continue;
                    }
                    byte fieldValue = ExtractField(paletteEntry, bitOffset, fieldSize);
                    bitOffset += fieldSize;

                    char color = colorOrder[colorIndex];
                    switch (color)
                    {
                        case 'R':
                            r = fieldValue;
                            break;
                        case 'G':
                            g = fieldValue;
                            break;
                        case 'B':
                            b = fieldValue;
                            break;
                        case 'A':
                            a = fieldValue;
                            break;
                        case 'X':
                            break; //skip
                    }
                }
                result[i] = (r, g, b, a);
            }

            return result;
        }

        public static (byte r, byte g, byte b, byte a)[] ParsePalette(uint[] palette, string pixelFormat)
        {
            //just assume its always ARGB8888 for now
            (byte r, byte g, byte b, byte a)[] result = new (byte r, byte g, byte b, byte a)[palette.Length];
            for (int i = 0; i < palette.Length; ++i)
            {
                uint paletteEntry = palette[i];
                byte a = (byte)((paletteEntry >> 24) & 0xFF);
                byte r = (byte)((paletteEntry >> 16) & 0xFF);
                byte b = (byte)((paletteEntry >> 8) & 0xFF);
                byte g = (byte)(paletteEntry & 0xFF);
                result[i] = (r, g, b, a);
            }
            return result;
        }

        public static (byte r, byte g, byte b, byte a)[] ParsePaletteABGR1555(byte[] palette)
        {
            (byte r, byte g, byte b, byte a)[] result = new (byte r, byte g, byte b, byte a)[palette.Length /2];
            for (int i = 0; i < result.Length; ++i)
            {
                int paletteIndex = i * 2;
                ushort entry = (ushort)(palette[paletteIndex] | (palette[paletteIndex + 1] << 8));
                byte a = ExtractField(entry, 0, 1);
                byte b = ExtractField(entry, 1, 5);
                byte g = ExtractField(entry, 6, 5);
                byte r = ExtractField(entry, 11, 5);
                result[i] = (r, g, b, a);
            }
            return result;
        }

        public static (byte r, byte g, byte b, byte a)[] ParsePaletteRGBA8888(byte[] palette)
        {
            (byte r, byte g, byte b, byte a)[] result = new (byte r, byte g, byte b, byte a)[palette.Length / 4];
            for (int i = 0; i < result.Length; ++i)
            {
                int paletteIndex = i * 4;
                byte r = palette[paletteIndex];
                byte g = palette[paletteIndex + 1];
                byte b = palette[paletteIndex + 2];
                byte a = palette[paletteIndex + 3];
                result[i] = (r, g, b, a);
            }
            return result;
        }

        public static (byte r, byte g, byte b, byte a)[] ParsePalette(byte[] palette, PaletteFormat format)
        {
            switch (format)
            {
                case PaletteFormat.ABGR1555:
                    /*ushort[] palette2Bytes = new ushort[palette.Length / 2];
                    for (int i = 0; i < palette2Bytes.Length; i++)
                    {
                        int offset = i * 2;
                        palette2Bytes[i] = (ushort)(palette[offset] | (palette[offset + 1] << 8));
                    }
                    return ParsePalette(palette2Bytes, "abgr1555");*/
                    return ParsePaletteABGR1555(palette);
                case PaletteFormat.RGBA8888:
                    return ParsePaletteRGBA8888(palette);
                default:
                    throw new Exception($"Unsupported palette format: {format}");
            }
        }

        public static byte[] _4To8BitIndices(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte b = bytes[i];
                byte highNibble = (byte)(b >> 4);
                byte lowNibble = (byte)(b & 0x0F);
                result[i * 2] = highNibble;
                result[i * 2 + 1] = lowNibble;
            }
            return result;
        }

        public static (byte r, byte g, byte b, byte a)[] GetImageColors(TexPS2Img image)
        {
            var palette = ParsePalette(image.palette, image.header.PaletteFormat);
            byte[] indices = image.imageData;
            if (image.header.Format == TextureFormat.Indexed4)
            {
                indices = _4To8BitIndices(image.imageData);
            }

            (byte r, byte g, byte b, byte a)[] outputColors = new (byte r, byte g, byte b, byte a)[indices.Length];
            for (int j = 0; j < indices.Length; ++j)
            {
                outputColors[j] = palette[indices[j]];
            }
            return outputColors;
        }


        static void Main(string[] args)
        {
            string inputFile = "I:\\Projects\\MTX Mototrax PRO Modding\\texture that doesnt work\\travis.tex";
            string fileNameNoExt = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile));

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StructReader(fs);

            TexPS2 tex = new TexPS2();
            tex.ReadFrom(fs);

            Console.WriteLine($"Done readin, file pointer at {fs.Position}");

            for (int i = 0; i < tex.images.Length; ++i)
            {
                var img = tex.images[i];
                var outputColors = GetImageColors(img);

                var outFile = $"{fileNameNoExt}_{i}_{img.header.Width}x{img.header.Height}.data";
                using var outFs = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                foreach (var col in outputColors)
                {
                    outFs.WriteByte(col.r);
                    outFs.WriteByte(col.g);
                    outFs.WriteByte(col.b);
                    outFs.WriteByte(col.a);
                }
            }
        }
    }
}
