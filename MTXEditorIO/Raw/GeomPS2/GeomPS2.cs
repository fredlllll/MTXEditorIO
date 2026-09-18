using MTXEditorIO.Util;
using System;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.GeomPS2
{
    public class GeomPS2 : IReadableWriteableFromStream
    {
        public GeomPS2Header header;
        public GeomPS2Record[] records = Array.Empty<GeomPS2Record>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<GeomPS2Header>();
            Console.WriteLine(Output.ToString(header));

            if (header.recordTableOffset + GeomPS2Record.RecordSize > (uint)stream.Length)
            {
                throw new InvalidDataException($"recordTableOffset 0x{header.recordTableOffset:X} is out of range for a file of length 0x{stream.Length:X}");
            }

            stream.Position = header.recordTableOffset;
            int numRecords = (int)((stream.Length - stream.Position) / GeomPS2Record.RecordSize);
            Console.WriteLine($"numRecords: {numRecords}");
            records = new GeomPS2Record[numRecords];
            for (int i = 0; i < numRecords; i++)
            {
                var rec = records[i] = new GeomPS2Record();
                rec.ReadFrom(reader);
            }
        }

        public void WriteTo(Stream stream)
        {
            throw new NotSupportedException("The PS2 geom format is still being reverse engineered, writing is not implemented yet.");
        }
    }
}