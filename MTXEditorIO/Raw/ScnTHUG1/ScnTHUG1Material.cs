using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1Material :IReadableWriteable
    {
        public ScnTHUG1MaterialHeader header = new ScnTHUG1MaterialHeader();
        public ScnTHUG1MaterialPass[] passes = Array.Empty<ScnTHUG1MaterialPass>();

        public void ReadFrom(BinaryReader reader)
        {
            header = new ScnTHUG1MaterialHeader();
            header.ReadFrom(reader);
            passes = new ScnTHUG1MaterialPass[header.fixedHeader.materialPasses];
            for(int i =0; i< passes.Length; ++i)
            {
                var pass = passes[i] = new ScnTHUG1MaterialPass(i == 0);
                pass.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            header.fixedHeader.materialPasses = (uint)passes.Length;
            header.WriteTo(writer);
            passes[0].isFirstPass = true;
            for (int i = 0; i < passes.Length; ++i)
            {
                passes[i].WriteTo(writer);
            }
        }

        public override string ToString()
        {
            return $"  {header}\n  {passes[0]}";
        }
    }
}
