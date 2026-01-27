using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1MaterialHeader : IReadableWriteable
    {
        public ScnTHUG1MaterialFixedHeader fixedHeader;
        public float grassHeight; //only if grassify
        public int grassLayers;//only if grassify
        public float specularPower;
        public Vec3 specularColor; //only if specularPower > 0

        public void ReadFrom(BinaryReader reader)
        {
            fixedHeader = reader.ReadStruct<ScnTHUG1MaterialFixedHeader>();
            if (fixedHeader.grassify)
            {
                grassHeight = reader.ReadSingle();
                grassLayers = reader.ReadInt32();
            }
            specularPower = reader.ReadSingle();
            if (specularPower > 0f)
            {
                specularColor = reader.ReadStruct<Vec3>();
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteStruct(fixedHeader);
            if (fixedHeader.grassify)
            {
                writer.Write(grassHeight);
                writer.Write(grassLayers);
            }
            writer.Write(specularPower);
            if (specularPower > 0f)
            {
                writer.WriteStruct(specularColor);
            }
        }

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}
