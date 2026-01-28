using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Util
{
    public static class BinaryReaderExtensions
    {
        public static T ReadStruct<T>(this Stream stream) where T : unmanaged
        {
            Span<byte> buffer = stackalloc byte[Marshal.SizeOf<T>()];
            int read = stream.Read(buffer);
            if (read < buffer.Length)
                throw new EndOfStreamException($"Not enough bytes to read {typeof(T).Name}");
            return MemoryMarshal.Read<T>(buffer);
        }

        public static T ReadStruct<T>(this BinaryReader reader) where T : unmanaged
        {
            return reader.BaseStream.ReadStruct<T>();
        }

        public static T[] ReadStructs<T>(this Stream stream, int count) where T : unmanaged
        {
            T[] values = new T[count];
            for (int i = 0; i < count; ++i)
            {
                values[i] = stream.ReadStruct<T>();
            }
            return values;
        }

        public static T[] ReadStructs<T>(this BinaryReader reader, int count) where T : unmanaged
        {
            return reader.BaseStream.ReadStructs<T>(count);
        }

        // read a null-terminated c string
        public static string ReadCString(this BinaryReader reader)
        {
            using var ms = new MemoryStream();
            byte b;
            while ((b = reader.ReadByte()) != 0)
            {
                ms.WriteByte(b);
            }
            return Encoding.ASCII.GetString(ms.ToArray());
        }

        // read a fixed length string, length prefixed as Int32
        public static string ReadFixedString(this BinaryReader reader)
        {
            int byteCount = reader.ReadInt32();
            var bytes = reader.ReadBytes(byteCount);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
