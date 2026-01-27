using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1MaterialPassVCWibble : IReadableWriteable
    {
        public int unknown;
        public Vec2[] keyframes = Array.Empty<Vec2>();

        public void ReadFrom(BinaryReader reader)
        {
            uint numKeys = reader.ReadUInt32();
            unknown = reader.ReadInt32();
            keyframes = new Vec2[numKeys];
            for (int i = 0; i < numKeys; ++i)
            {
                keyframes[i] = reader.ReadStruct<Vec2>();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((uint)keyframes.Length);
            writer.Write(unknown);
            for (int i = 0; i < keyframes.Length; ++i)
            {
                writer.WriteStruct(keyframes[i]);
            }
        }
    }
}