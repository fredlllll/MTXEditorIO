using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1 : IReadableWriteableFromStream
    {
        public ScnTHUG1Header header;
        public ScnTHUG1Material[] materials = Array.Empty<ScnTHUG1Material>();
        public ScnTHUG1Sector[] sectors = Array.Empty<ScnTHUG1Sector>();

        public void ReadFrom(Stream stream)
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            header = reader.ReadStruct<ScnTHUG1Header>();
            Console.WriteLine(Output.ToString(header));

            uint numMaterials = reader.ReadUInt32();
            Console.WriteLine($"numMaterials: {numMaterials}");
            materials = new ScnTHUG1Material[numMaterials];
            for (int i = 0; i < numMaterials; i++)
            {
                var mat = materials[i] = new ScnTHUG1Material();
                mat.ReadFrom(reader);
            }

            uint numSectors = reader.ReadUInt32();
            Console.WriteLine($"numSectors: {numSectors}");
            sectors = new ScnTHUG1Sector[numSectors];
            for (int i = 0; i < numSectors; i++)
            {
                var sec = sectors[i] = new ScnTHUG1Sector();
                sec.ReadFrom(reader);
            }
            uint numUnknown = reader.ReadUInt32(); //one 0 uint at the end, either a 0 terminator (unlikely) or a counter for an additional type of structure that has been omitted from the final file format
        }

        public void WriteTo(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
