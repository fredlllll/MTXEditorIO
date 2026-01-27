using MTXEditorIO.Raw.Shared;
using System;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.ScnTHUG1
{
    [Flags]
    public enum MaterialPassHeaderFlags : uint
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
        public MaterialPassHeaderFlags flags;
        public OneByteBoolean hasColor;
        public Vec3 passColor;
        public BlendMode blendMode;
        public uint blendFixedAlpha; //only LSB is used?
        public UVAdressing uAddressing;
        public UVAdressing vAddressing;
        public Vec2 envmapMultiples;
        public uint filteringMode;
    }
}