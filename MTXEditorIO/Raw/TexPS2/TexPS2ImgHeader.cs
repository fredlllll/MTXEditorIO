using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public enum PixelFormat : uint
    {
        RGBA8888 = 0,
        ABGR1555 = 2,
        Indexed8 = 19,
        Indexed4 = 20,
    }

    public interface ITexPS2ImgHeader
    {
        uint Checksum { get; set; }
        uint WidthPowOfTwo { get; set; }
        uint HeightPowOfTwo { get; set; }
        uint Width => (uint)(1 << (int)WidthPowOfTwo);
        uint Height => (uint)(1 << (int)HeightPowOfTwo);
        PixelFormat Format { get; set; }
        PixelFormat PaletteFormat { get; set; }
        uint MipMapLevels { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexPS2FirstImgHeader : ITexPS2ImgHeader
    {
        public uint checksum;
        public uint widthPowOfTwo;
        public uint heightPowOfTwo;
        public PixelFormat format;
        public PixelFormat paletteFormat;
        public uint mipMapLevels;
        public uint unknown1;
        public uint unknown2;

        public uint Checksum { get { return checksum; } set { checksum = value; } }
        public uint WidthPowOfTwo { get { return widthPowOfTwo; } set { widthPowOfTwo = value; } }
        public uint HeightPowOfTwo { get { return heightPowOfTwo; } set { heightPowOfTwo = value; } }
        public PixelFormat Format { get { return format; } set { format = value; } }
        public PixelFormat PaletteFormat { get { return paletteFormat; } set { paletteFormat = value; } }
        public uint MipMapLevels { get { return mipMapLevels; } set { mipMapLevels = value; } }

        public override string ToString()
        {
            return $"checksum: {checksum}, widthPowOfTwo: {widthPowOfTwo}, heightPowOfTwo: {heightPowOfTwo}, format: {format}, paletteFormat: {paletteFormat}, mipMapLevels: {mipMapLevels}, unknown1: {unknown1}, unknown2: {unknown2}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TexPS2ImgHeader : ITexPS2ImgHeader
    {
        public uint unknown1;
        public uint checksum;
        public uint widthPowOfTwo;
        public uint heightPowOfTwo;
        public PixelFormat format;
        public PixelFormat paletteFormat;
        public uint mipMapLevels;
        public uint unknown2;

        public uint Checksum { get { return checksum; } set { checksum = value; } }
        public uint WidthPowOfTwo { get { return widthPowOfTwo; } set { widthPowOfTwo = value; } }
        public uint HeightPowOfTwo { get { return heightPowOfTwo; } set { heightPowOfTwo = value; } }
        public PixelFormat Format { get { return format; } set { format = value; } }
        public PixelFormat PaletteFormat { get { return paletteFormat; } set { paletteFormat = value; } }
        public uint MipMapLevels { get { return mipMapLevels; } set { mipMapLevels = value; } }

        public override string ToString()
        {
            return $"checksum: {checksum}, widthPowOfTwo: {widthPowOfTwo}, heightPowOfTwo: {heightPowOfTwo}, format: {format}, paletteFormat: {paletteFormat}, mipMapLevels: {mipMapLevels}, unknown1: {unknown1}, unknown2: {unknown2}";
        }
    }
}
