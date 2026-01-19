using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using MTXEditorIO.Raw.Pre;
using MTXEditorIO.Raw.TexPC;
using MTXEditorIO.Util;
using MTXEditorIO.Util.Hashing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Data;
using System.Numerics;
using System.Text.Unicode;

namespace Test
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "I:\\Projects\\MTX Mototrax PRO Modding\\MTX Mototrax PRO\\data\\pre\\TrackEditor_Pcs2Zpre";
            inputFile = "I:\\Projects\\MTX Mototrax PRO Modding\\MTX Mototrax PRO\\data\\pre\\casshirt.pre";
            string fileNameNoExt = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_dir");

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Pre pre = new Pre();
            pre.ReadFrom(fs);
            Directory.CreateDirectory(fileNameNoExt);

            for (int i = 0; i < pre.items.Length; i++)
            {
                var item = pre.items[i];

                Console.WriteLine($"File {i}: {item.fileName} compressed: {item.header.IsCompressed} deflated: {item.header.DeflatedSize} inflated: {item.header.InflatedSize}");
            }

            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");
        }
    }
}
