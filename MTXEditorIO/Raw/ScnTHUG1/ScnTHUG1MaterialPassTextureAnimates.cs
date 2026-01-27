using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1MaterialPassTextureAnimates : IReadableWriteable
    {
        public int numKeyframes;
        public int period;
        public int iterations;
        public int phase;
        public ScnTHUG1MaterialPassTextureAnimatesKeyframe[] keyframes = Array.Empty<ScnTHUG1MaterialPassTextureAnimatesKeyframe>();
        public void ReadFrom(BinaryReader reader)
        {
            numKeyframes = reader.ReadInt32();
            period = reader.ReadInt32();
            iterations = reader.ReadInt32();
            phase = reader.ReadInt32();

            keyframes = new ScnTHUG1MaterialPassTextureAnimatesKeyframe[numKeyframes];
            for (int i = 0; i < numKeyframes; ++i)
            {
                keyframes[i] = reader.ReadStruct<ScnTHUG1MaterialPassTextureAnimatesKeyframe>();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            numKeyframes = keyframes.Length;
            writer.Write(numKeyframes);
            writer.Write(period);
            writer.Write(iterations);
            writer.Write(phase);
            for (int i = 0; i < keyframes.Length; ++i)
            {
                writer.WriteStruct(keyframes[i]);
            }
        }
    }
}