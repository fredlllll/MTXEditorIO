using MTXEditorIO.Raw.Zqb.QbChunks;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb
{
    public class Zqb : IReadableWriteableFromStream
    {
        public BaseChunk[] Chunks = System.Array.Empty<BaseChunk>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);
            //file is made up of chunks. each chunks first byte tells us the type (opcode)
            List<BaseChunk> chunks = new List<BaseChunk>();
            while (stream.Position < stream.Length)
            {
                QbChunkCode chunkType = (QbChunkCode)reader.ReadByte();
                var t = codeToType[chunkType];
                var obj = (BaseChunk)Activator.CreateInstance(t);
                obj.ReadFrom(reader);
                chunks.Add(obj);
                //Console.WriteLine($"Read chunk of type {chunkType} at position {stream.Position}: {Output.ToString(obj)}");
            }
            Chunks = chunks.ToArray();
        }

        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            foreach (var chunk in Chunks)
            {
                QbChunkCode chunkCode = typeToCode[chunk.GetType()];
                writer.Write((byte)chunkCode);
                chunk.WriteTo(writer);
            }
        }

        static Dictionary<QbChunkCode, Type> codeToType = new Dictionary<QbChunkCode, Type>();
        static Dictionary<Type, QbChunkCode> typeToCode = new Dictionary<Type, QbChunkCode>();

        static Zqb()
        {
            var names = Enum.GetNames(typeof(QbChunkCode));
            foreach (var name in names)
            {
                var code = (QbChunkCode)Enum.Parse(typeof(QbChunkCode), name);
                var type = Type.GetType("MTXEditorIO.Raw.Zqb.QbChunks." + name);
                if (type == null)
                {
                    throw new Exception("Could not find type for QbChunkCode: " + name);
                }
                codeToType[code] = type;
                typeToCode[type] = code;
            }
        }
    }
}
