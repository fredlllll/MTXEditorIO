using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.TexPC
{
    public class TexPCImg : IReadableWriteable
    {
        public TexPCImgHeader header;
        public TexPCImgLevel[] levels = Array.Empty<TexPCImgLevel>();

        public void ReadFrom(BinaryReader reader)
        {
            header = reader.ReadStruct<TexPCImgHeader>();
            Console.WriteLine(header);

            levels = new TexPCImgLevel[header.levels];
            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i] = new TexPCImgLevel();
                level.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteStruct(header);
            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                level.WriteTo(writer);
            }
        }
    }
}
