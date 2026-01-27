using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1SectorMeshLodLevel : IReadableWriteable
    {
        public ushort[] vertIndices = Array.Empty<ushort>();

        public void ReadFrom(BinaryReader reader)
        {
            uint numIndices = reader.ReadUInt32();
            vertIndices = new ushort[numIndices];
            for (int i = 0; i < numIndices; i++)
            {
                vertIndices[i] = reader.ReadUInt16();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((uint)vertIndices.Length);
            for (int i = 0; i < vertIndices.Length; i++)
            {
                writer.Write(vertIndices[i]);
            }
        }
    }
}