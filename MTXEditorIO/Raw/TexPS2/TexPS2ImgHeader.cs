using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.TexPS2
{
    public class TexPS2ImgHeader :IReadableWriteable
    {
        //extended part of header, is present if first int is not 0
        public uint firstChecksum;
        public uint unknown1;
        public uint unknownChecksum; //either 0 or 0x44bb8000, could also be two shorts
        public uint unknownNum;
        public uint unknown2;

        public uint secondChecksum;
        public uint widthPowOfTwo;
        public uint heightPowOfTwo;
        public PixelFormat format;
        public PixelFormat paletteFormat;
        public ushort mipMapLevels;
        public ushort noDataIfNotZero;

        public uint Width => (uint)(1 << (int)widthPowOfTwo);
        public uint Height => (uint)(1 << (int)heightPowOfTwo);

        public void ReadFrom(BinaryReader reader)
        {
            uint firstInt = reader.ReadUInt32();
            if(firstInt !=0)
            {
                firstChecksum = firstInt;
                unknown1 = reader.ReadUInt32();
                unknownChecksum = reader.ReadUInt32();
                unknownNum = reader.ReadUInt32();
                unknown2 = reader.ReadUInt32();
            }

            secondChecksum = reader.ReadUInt32();
            widthPowOfTwo = reader.ReadUInt32();
            heightPowOfTwo = reader.ReadUInt32();
            format = (PixelFormat)reader.ReadUInt32();
            paletteFormat = (PixelFormat)reader.ReadUInt32();
            mipMapLevels = reader.ReadUInt16();
            noDataIfNotZero = reader.ReadUInt16();
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public enum PixelFormat : uint
    {
        RGBA8888 = 0,
        ABGR1555 = 2,
        Indexed8 = 19,
        Indexed4 = 20,
    }
}
