using MTXEditorIO.Util;
using System;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Img
{
    public class Img : IReadableWriteableFromStream
    {
        public ImgHeader header;
        public RGBA8888Color[] palette = Array.Empty<RGBA8888Color>();
        public byte[] data = Array.Empty<byte>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);
            header = reader.ReadStruct<ImgHeader>();
            int pixelNum = (int)(header.imageDataWidth * header.imageDataHeight);
            int dataSize = pixelNum * 4;
            if (header.paletteSize > 0)
            {
                //always encoded as RGBA8888
                palette = reader.ReadStructs<RGBA8888Color>((int)header.paletteSize / 4);
                dataSize = pixelNum;
            }
            data = reader.ReadBytes(dataSize);
        }

        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            header.version = 2;
            header.paletteSize = (uint)palette.Length;
            header.someFlags = SomeFlags.flag512_____; //512 in many files. idk what it means though
            if (palette.Length > 0)
            {
                header.pixelFormat = ImgPixelFormat.Indexed8;
            }
            else
            {
                header.pixelFormat = ImgPixelFormat.BGRA8888;
            }
            writer.WriteStruct(header);
            if (palette.Length > 0)
            {
                writer.WriteStructs(palette);
            }
            writer.Write(data);
        }
    }
}
