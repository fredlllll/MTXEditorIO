using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.Img
{
    public class Img : IReadableWriteableFromStream
    {
        public ImgHeader header;
        public byte[] palette = Array.Empty<byte>();
        public byte[] data = Array.Empty<byte>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);
            header = reader.ReadStruct<ImgHeader>();

            if (header.palSize > 0)
            {
                //always encoded as RGBA8888
                palette = reader.ReadBytes((int)header.palSize);
            }
            var dataSize = reader.BaseStream.Length - reader.BaseStream.Position;
            data = reader.ReadBytes((int)dataSize);
        }

        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            header.palSize = (uint)palette.Length;
            header.fileSize = (uint)(Marshal.SizeOf<ImgHeader>() + palette.Length + data.Length);
            writer.WriteStruct(header);
            if (palette.Length > 0)
            {
                writer.Write(palette);
            }
            writer.Write(data);
        }
    }
}
