using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Zqb.QbChunks
{
    public class Random : BaseChunk
    {
        public Random() : base(QbChunkCode.Random) { }

        protected Random(QbChunkCode code) : base(code) { }

        public int[] pointers = System.Array.Empty<int>();

        public override void ReadFrom(BinaryReader reader)
        {
            int numPointers = reader.ReadInt32();
            pointers = new int[numPointers];
            for (int i = 0; i < numPointers; i++)
            {
                pointers[i] = reader.ReadInt32();
            }
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write(pointers.Length);
            for (int i = 0; i < pointers.Length; i++)
            {
                writer.Write(pointers[i]);
            }
        }

        public override string ToString()
        {
            return "random";
        }
    }
}
