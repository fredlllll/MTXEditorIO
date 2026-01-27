using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1SectorMesh : IReadableWriteable
    {
        public ScnTHUG1SectorMeshHeader header;
        public ScnTHUG1SectorMeshLodLevel[] lodLevels = Array.Empty<ScnTHUG1SectorMeshLodLevel>();

        public void ReadFrom(BinaryReader reader)
        {
            header = reader.ReadStruct<ScnTHUG1SectorMeshHeader>();
            lodLevels = new ScnTHUG1SectorMeshLodLevel[header.numLodLevels];
            for (int i = 0; i < lodLevels.Length; i++)
            {
                var ll = lodLevels[i] = new ScnTHUG1SectorMeshLodLevel();
                ll.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            header.numLodLevels = (uint)lodLevels.Length;
            writer.WriteStruct(header);
            for (int i = 0; i < lodLevels.Length; i++)
            {
                lodLevels[i].WriteTo(writer);
            }
        }
    }
}