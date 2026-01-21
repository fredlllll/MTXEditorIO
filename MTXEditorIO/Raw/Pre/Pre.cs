using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTXEditorIO.Raw.Pre
{
    public class Pre : IReadableWriteableFromStream
    {
        public PreHeader header;
        public PreItem[] items = Array.Empty<PreItem>();

        public void ReadFrom(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.ASCII, true);
            header = reader.ReadStruct<PreHeader>();

            items = new PreItem[header.itemCount];
            for (int i = 0; i < header.itemCount; i++)
            {
                var item = items[i] = new PreItem(header.version);
                item.ReadFrom(reader);
            }
        }

        public void WriteTo(Stream stream)
        {
            long startPos = stream.Position;
            using var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            header.itemCount = (uint)items.Length;
            header.id = 0xABCD;
            //ill leave version to the user
            writer.WriteStruct(header);
            Console.WriteLine($"writing {header.itemCount} items");
            for (int i = 0; i < header.itemCount; i++)
            {
                items[i].WriteTo(writer);
            }
            header.fileSize = (uint)(stream.Position - startPos);
            long endPos = stream.Position;
            stream.Position = startPos;
            writer.Write(header.fileSize);
            stream.Position = endPos;
        }
    }
}
