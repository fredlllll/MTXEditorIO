using MTXEditorIO.Raw.TexPC;
using MTXEditorIO.Util;
using MTXEditorIO.Util.Compression;
using MTXEditorIO.Util.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Pre
{
    public class PreItem : IReadableWriteable
    {
        private PreVersion preVersion;

        public IPreItemHeader header = new PreItemHeader2();
        public string fileName = string.Empty;
        public byte[] uncompressedData = Array.Empty<byte>();

        public PreItem(PreVersion preVersion)
        {
            this.preVersion = preVersion;
            switch (preVersion)
            {
                case PreVersion.pre2:
                    header = new PreItemHeader2();
                    break;
                case PreVersion.pre3:
                case PreVersion.pre4:
                    header = new PreItemHeader3_4();
                    break;
                default:
                    throw new Exception("unsupported version: " + preVersion);
            }
        }

        public uint GetFileNameCrc32()
        {
            //for some unfathomable reason the crc is always over the lowercase version of the string
            return Crc32IEEE.Hash(StringUtils.GetBytes(fileName.ToLower()));
        }

        public void ReadFrom(BinaryReader reader)
        {
            if (preVersion == PreVersion.pre2)
            {
                header = reader.ReadStruct<PreItemHeader2>();
            }
            else if (preVersion == PreVersion.pre3 || preVersion == PreVersion.pre4)
            {
                header = reader.ReadStruct<PreItemHeader3_4>();
            }
            else
            {
                throw new Exception("unsupported version " + preVersion);
            }

            byte[] fileNameBytes = reader.ReadBytes((int)header.FileNameLength);
            fileName = StringUtils.GetStringSafe(fileNameBytes);
            if (header.FileNameCrc != 0 && GetFileNameCrc32() != header.FileNameCrc)
            {
                Console.WriteLine("Warning: filename crc missmatch for " + fileName);
            }

            uint totalDataLength = 0;
            uint usableDataLength = 0;
            if (header.IsCompressed)
            {
                //wtf kinda math is that??
                //i think its trying to align the size with the next multiple of 4
                totalDataLength = (header.DeflatedSize + 3) & 0xFFFFFFFCu;
                usableDataLength = header.DeflatedSize;
            }
            else
            {
                totalDataLength = (header.InflatedSize + 3) & 0xFFFFFFFCu;
                usableDataLength = header.InflatedSize;
            }
            var data = reader.ReadBytes((int)usableDataLength);
            reader.ReadBytes((int)(totalDataLength - usableDataLength)); //discard superflous bytes
            if (header.IsCompressed)
            {
                uncompressedData = new Lzss().Decompress(data);
            }
            else
            {
                uncompressedData = data;
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            header.FileNameCrc = GetFileNameCrc32();
            var fileNameBytes = StringUtils.GetBytes(fileName);
            header.FileNameLength = (uint)fileNameBytes.Length;
            header.InflatedSize = (uint)uncompressedData.Length;
            var compressedData = new Lzss().Compress(uncompressedData);
            header.DeflatedSize = (uint)compressedData.Length;
            uint totalDataLength = (header.DeflatedSize + 3) & 0xFFFFFFFCu; //align to 4 bytes

            if (header is PreItemHeader2 header2)
            {
                writer.WriteStruct(header2);
            }
            else if (header is PreItemHeader3_4 header3_4)
            {
                writer.WriteStruct(header3_4);
            }

            writer.Write(fileNameBytes);
            writer.Write(compressedData);
            writer.Write(new byte[totalDataLength - header.DeflatedSize]); //write alignment bytes
        }
    }
}
