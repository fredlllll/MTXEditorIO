using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialFixedHeader
    {
        public uint checksum;
        public uint materialNameChecksum;
        public uint materialPasses;
        public uint alphaCutoff; // only LSB is used, perhaps this field isnt correct
        public OneByteBoolean sorted;
        public float drawOrder;
        public OneByteBoolean singleSided;
        public OneByteBoolean noBackfaceCulling;
        public int zBias;
        public OneByteBoolean grassify;

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }

    public class ScnTHUG1MaterialHeader : IReadableWriteable
    {
        public ScnTHUG1MaterialFixedHeader fixedHeader;
        public float grassHeight; //only if grassify
        public int grassLayers;//only if grassify
        public float specularPower;
        public Vec3 specularColor;

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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Output.ToString(this);
        }
    }
}
