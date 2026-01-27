using MTXEditorIO.Util;
using System;
using System.IO;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    public class ScnTHUG1MaterialPass : IReadableWriteable
    {
        public bool isFirstPass; //apparently only the first material post can have VC wibbles

        public ScnTHUG1MaterialPassHeader header;
        public ScnTHUG1MaterialPassUVWibble uvWibble;
        public ScnTHUG1MaterialPassVCWibble[] vcWibbles = Array.Empty<ScnTHUG1MaterialPassVCWibble>();
        public ScnTHUG1MaterialPassTextureAnimates textureAnimates = new ScnTHUG1MaterialPassTextureAnimates();
        public ScnTHUG1MaterialPassFooter footer;


        public ScnTHUG1MaterialPass(bool isFirstPass)
        {
            this.isFirstPass = isFirstPass;
        }

        public void ReadFrom(BinaryReader reader)
        {
            header = reader.ReadStruct<ScnTHUG1MaterialPassHeader>();

            if (header.flags.HasFlag(MaterialPassHeaderFlags.UV_WIBBLE))
            {
                uvWibble = reader.ReadStruct<ScnTHUG1MaterialPassUVWibble>();
            }
            if (isFirstPass && header.flags.HasFlag(MaterialPassHeaderFlags.VC_WIBBLE))
            {
                //this is skipped in blender plugin
                uint numWibbles = reader.ReadUInt32();
                vcWibbles = new ScnTHUG1MaterialPassVCWibble[numWibbles];
                for (int i = 0; i < numWibbles; ++i)
                {
                    var wibble = vcWibbles[i] = new ScnTHUG1MaterialPassVCWibble();
                    wibble.ReadFrom(reader);
                }
            }
            if (header.flags.HasFlag(MaterialPassHeaderFlags.PASS_TEXTURE_ANIMATES))
            {
                textureAnimates.ReadFrom(reader);
            }
            footer = reader.ReadStruct<ScnTHUG1MaterialPassFooter>();
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteStruct(header);
            if (header.flags.HasFlag(MaterialPassHeaderFlags.UV_WIBBLE))
            {
                writer.WriteStruct(uvWibble);
            }
            if (isFirstPass && header.flags.HasFlag(MaterialPassHeaderFlags.VC_WIBBLE))
            {
                writer.Write((uint)vcWibbles.Length);
                for (int i = 0; i < vcWibbles.Length; ++i)
                {
                    vcWibbles[i].WriteTo(writer);
                }
            }
            if (header.flags.HasFlag(MaterialPassHeaderFlags.PASS_TEXTURE_ANIMATES))
            {
                textureAnimates.WriteTo(writer);
            }
            writer.WriteStruct(footer);
        }
    }
}