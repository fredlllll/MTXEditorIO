using MTXEditorIO.Raw.Pre;

namespace UnpackPre
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFile = Path.GetFullPath(args[0]);
            string outputDir = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile));

            using var fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Pre pre = new Pre();
            pre.ReadFrom(fs);
            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}");

            for (int i = 0; i < pre.items.Length; i++)
            {
                var item = pre.items[i];
                var outputPath = Path.Combine(outputDir, item.fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                Console.WriteLine($"Writing File {i}: {item.fileName}");
                File.WriteAllBytes(outputPath, item.uncompressedData);
            }
        }
    }
}
