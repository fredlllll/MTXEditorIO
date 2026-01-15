using System;
using System.Collections.Generic;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public static class ImageConversion
    {
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

        public static (byte r, byte g, byte b, byte a)[] ParsePaletteABGR1555(byte[] palette)
        {
            (byte r, byte g, byte b, byte a)[] result = new (byte r, byte g, byte b, byte a)[palette.Length / 2];
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

        static byte ExpandTo8(byte fieldValue, int fieldSize)
        {
            int max = (1 << fieldSize) - 1;
            return (byte)((fieldValue * 255 + (max >> 1)) / max);
        }

        static byte ExtractField(ushort value, int bitOffset, int fieldSize)
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
    }
}
