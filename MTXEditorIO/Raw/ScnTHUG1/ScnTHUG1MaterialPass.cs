using MTXEditorIO.Raw.Shared;
using MTXEditorIO.Util;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [Flags]
    public enum PassFlags : uint
    {
        UV_WIBBLE = 1 << 0,
        VC_WIBBLE = 1 << 1,
        TEXTURED = 1 << 2,
        ENVIRONMENT = 1 << 3,
        DECAL = 1 << 4,
        SMOOTH = 1 << 5,
        TRANSPARENT = 1 << 6,
        PASS_COLOR_LOCKED = 1 << 7,
        SPECULAR = 1 << 8,
        BUMP_SIGNED_TEXTURE = 1 << 9,
        BUMP_LOAD_MATRIX = 1 << 10,
        PASS_TEXTURE_ANIMATES = 1 << 11,
        PASS_IGNORE_VERTEX_ALPHA = 1 << 12,
        EXPLICIT_UV_WIBBLE = 1 << 14,
        REPLACE = 1 << 15,
        STATIC = 1 << 16,
        ALLOW_RECOLOR = 1 << 25,
        FIXED_SCALE = 1 << 26,
        WATER_EFFECT = 1 << 27,
        NO_MAT_COL_MOD = 1 << 28,
        NORMAL_TEST = 1 << 29,
    }

    public enum UVAdressing
    {
        Repeat = 0,
        Clamp = 1,
        Border = 2,
    }

    public enum BlendMode
    {
        DIFFUSE = 0,
        ADD = 1,
        ADD_FIXED = 2,
        SUBTRACT = 3,
        SUB_FIXED = 4,
        BLEND = 5,
        BLEND_FIXED = 6,
        MODULATE = 7,
        MODULATE_FIXED = 8,
        BRIGHTEN = 9,
        BRIGHTEN_FIXED = 10,
        GLOSS_MAP = 11,
        BLEND_PREVIOUS_MASK = 12,
        BLEND_INVERSE_PREVIOUS_MASK = 13,
        UNKNOWN1 = 14,
        MODULATE_COLOR = 15,
        UNKNOWN2 = 16,
        ONE_INV_SRC_ALPHA = 17,
        OVERLAY = 18,
        NORMAL_MAP = 19,
        LIGHTMAP = 20,
        NORMAL_ROUGH = 21,
        MASK = 22,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassHeader
    {
        public uint checksum;
        public PassFlags flags;
        public OneByteBoolean hasColor;
        public Vec3 passColor;
        public BlendMode blendMode;
        public uint blendFixedAlpha; //only LSB is used?
        public UVAdressing uAddressing;
        public UVAdressing vAddressing;
        public Vec2 envmapMultiples;
        public uint filteringMode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassUVWibble
    {
        public Vec2 velocity;
        public Vec2 frequency;
        public Vec2 amplitude;
        public Vec2 phase;
    }

    public class ScnTHUG1MaterialPassVCWibble : IReadableWriteable
    {
        public int unknown;
        public Vec2[] keyframes;

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
            throw new NotImplementedException();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassTextureAnimatesKeyframe
    {
        public uint time;
        public uint image; //probably a pointer into a list or dictionary or tex file?
    }

    public class ScnTHUG1MaterialPassTextureAnimates : IReadableWriteable
    {
        public int numKeyframes;
        public int period;
        public int iterations;
        public int phase;
        public ScnTHUG1MaterialPassTextureAnimatesKeyframe[] keyframes;
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
            throw new NotImplementedException();
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScnTHUG1MaterialPassFooter
    {
        public uint mmag;
        public uint mmin;
        public float lodBias;
        public float l;
    }

    public class ScnTHUG1MaterialPass : IReadableWriteable
    {
        public bool isFirstPass;

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

            if (header.flags.HasFlag(PassFlags.UV_WIBBLE))
            {
                uvWibble = reader.ReadStruct<ScnTHUG1MaterialPassUVWibble>();
            }
            if (isFirstPass && header.flags.HasFlag(PassFlags.VC_WIBBLE))
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
            if (header.flags.HasFlag(PassFlags.PASS_TEXTURE_ANIMATES))
            {
                textureAnimates.ReadFrom(reader);
            }
            footer = reader.ReadStruct<ScnTHUG1MaterialPassFooter>();
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}