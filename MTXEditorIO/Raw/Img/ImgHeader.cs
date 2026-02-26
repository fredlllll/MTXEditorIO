using MTXEditorIO.Raw.TexPS2;
using System.Runtime.InteropServices;

namespace MTXEditorIO.Raw.Img
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBA8888Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }

    public enum ImgPixelFormat : uint
    {
        BGRA8888 = 0,
        Indexed8 = 19,
    }

    public enum SomeFlags:uint
    {
        //no idea what these values mean, but these are the only two values i found for this field
        flag512_____ = 512,
        flag12386304 = 12386304,
    }

    public enum UnknownFlags:uint
    {
        flag0_______ = 0,
        flag12390240 = 12390240,
        flag12386680 = 12386680,
        flag14549064 = 14549064,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImgHeader
    {
        internal uint version; //= 2
        public SomeFlags someFlags;
        public uint imageDataWidth;
        public uint imageDataHeight;
        public ImgPixelFormat pixelFormat;
        public UnknownFlags unknownFlags;
        public ushort imageWidth;
        public ushort imageHeight;
        internal uint paletteSize; //in bytes

        public int DataSize
        {
            get { return (int)(imageDataWidth * imageDataHeight); }
        }

        public void SetWidth(int value)
        {
            imageDataWidth = (uint)value;
            imageWidth = (ushort)value;
        }

        public void SetHeight(int value)
        {
            imageDataHeight = (uint)value;
            imageHeight = (ushort)value;
        }

        public override string ToString()
        {
            return $"{someFlags} {pixelFormat} {unknownFlags} data:{imageDataWidth}x{imageDataHeight} img:{imageWidth}x{imageHeight}";
        }
    }
}
