using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.TexPS2
{
    public enum TextureFormat : uint
    {
        Indexed8 = 19,
        Indexed4 = 20,
    }

    public enum PaletteFormat : uint
    {
        RGBA8888 = 0,
        ABGR1555 = 2,
    }

    public interface ITexPS2ImgHeader
    {
        uint Checksum { get; }
        uint WidthPowOfTwo { get; }
        uint HeightPowOfTwo { get; }
        uint Width => (uint)(1 << (int)WidthPowOfTwo);
        uint Height => (uint)(1 << (int)HeightPowOfTwo);
        TextureFormat Format { get; }
        PaletteFormat PaletteFormat { get; }
        uint MipMapLevels { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TexPS2FirstImgHeader : ITexPS2ImgHeader
    {
        public uint checksum;
        public uint widthPowOfTwo;
        public uint heightPowOfTwo;
        public TextureFormat format;
        public PaletteFormat paletteFormat;
        public uint mipMapLevels;
        public uint unknown1;
        public uint unknown2;

        public uint Checksum => checksum;
        public uint WidthPowOfTwo => widthPowOfTwo;
        public uint HeightPowOfTwo => heightPowOfTwo;
        public TextureFormat Format => format;
        public PaletteFormat PaletteFormat => paletteFormat;
        public uint MipMapLevels => mipMapLevels;

        public override string ToString()
        {
            return $"checksum: {checksum}, widthPowOfTwo: {widthPowOfTwo}, heightPowOfTwo: {heightPowOfTwo}, format: {format}, paletteFormat: {paletteFormat}, mipMapLevels: {mipMapLevels}, unknown1: {unknown1}, unknown2: {unknown2}";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TexPS2ImgHeader : ITexPS2ImgHeader
    {
        public uint unknown1;
        public uint checksum;
        public uint widthPowOfTwo;
        public uint heightPowOfTwo;
        public TextureFormat format;
        public PaletteFormat paletteFormat;
        public uint mipMapLevels;
        public uint unknown2;

        public uint Checksum => checksum;
        public uint WidthPowOfTwo => widthPowOfTwo;
        public uint HeightPowOfTwo => heightPowOfTwo;
        public TextureFormat Format => format;
        public PaletteFormat PaletteFormat => paletteFormat;
        public uint MipMapLevels => mipMapLevels;

        public override string ToString()
        {
            return $"checksum: {checksum}, widthPowOfTwo: {widthPowOfTwo}, heightPowOfTwo: {heightPowOfTwo}, format: {format}, paletteFormat: {paletteFormat}, mipMapLevels: {mipMapLevels}, unknown1: {unknown1}, unknown2: {unknown2}";
        }
    }
}
