using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Util
{
    public static class BinaryWriterExtensions
    {
        public static void WriteStruct<T>(this Stream stream, T obj) where T : unmanaged
        {
            Span<byte> buffer = stackalloc byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write(buffer, ref obj);
            stream.Write(buffer);
        }

        public static void WriteStruct<T>(this BinaryWriter writer, T obj) where T : unmanaged
        {
            writer.BaseStream.WriteStruct(obj);
        }

        public static void WriteStructs<T>(this Stream stream, T[] values) where T : unmanaged
        {
            for (int i = 0; i < values.Length; ++i)
            {
                stream.WriteStruct(values[i]);
            }
        }

        public static void WriteStructs<T>(this BinaryWriter writer, T[] values) where T : unmanaged
        {
            writer.BaseStream.WriteStructs(values);
        }

        public static void PadTo(this Stream stream, long positionToPadTo)
        {
            int paddingBytes = 0;
            checked
            {
                paddingBytes = (int)(positionToPadTo - stream.Position);
            }
            if (paddingBytes < 0)
            {
                throw new Exception("paddingBytes cant be negative");
            }
            Span<byte> buffer = stackalloc byte[paddingBytes];
            stream.Write(buffer);
        }

        public static void PadTo(this BinaryWriter writer, long positionToPadTo)
        {
            writer.BaseStream.PadTo(positionToPadTo);
        }

        //write a null-terminated c string
        public static void WriteCString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            writer.Write(bytes);
            writer.Write((byte)0x00);
        }

        //write a length-prefixed string, with length as Int32
        public static void WriteFixedString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
    }
}
