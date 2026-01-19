using MTXEditorIO.Raw.Pre;

namespace PackPre
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFolder = Path.GetFullPath(args[0]);
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine("Input folder doesnt exist");
                return;
            }

            //just gonna go with version 4 for everything for now till a need for anything else arises

            List<PreItem> preItems = new List<PreItem>();
            DirectoryInfo dir = new DirectoryInfo(inputFolder);
            foreach (var entry in dir.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                //make sure we always use \ as dir seperator, in case someone is crazy enough to run this on linux
                var relativePath = Path.GetRelativePath(dir.FullName, entry.FullName).Replace(Path.DirectorySeparatorChar, '\\');

                var item = new PreItem(PreVersion.pre4);
                item.header = new PreItemHeader3_4();
                item.fileName = relativePath;
                item.uncompressedData = File.ReadAllBytes(entry.FullName);
                Console.WriteLine($"adding {relativePath}");
                preItems.Add(item);
            }

            var outputFile = inputFolder + ".pre";

            Pre pre = new Pre();
            pre.header.version = PreVersion.pre4;
            pre.items = preItems.ToArray();
            using var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None);
            pre.WriteTo(fs);
            Console.WriteLine($"Done writing {fs.Position} bytes");
        }
    }
}
