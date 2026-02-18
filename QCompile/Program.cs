using CommandLine;
using MTXEditorIO.Raw.Zqb;
using MTXEditorIO.Raw.Zqb.QbChunks;

namespace QCompile
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineArgs>(args).WithParsed<CommandLineArgs>(Main);
        }

        static void Main(CommandLineArgs args)
        {
            if (!args.compile)
            {
                Decompile(Path.GetFullPath(args.InputFile));
            }
            else
            {
                Compile(Path.GetFullPath(args.InputFile));
            }
        }

        static void Decompile(string filePath)
        {
            if (filePath.EndsWith(".qcode"))
            {
                //to support drag and drop we do an extra check here to see if its a code file to be compiled instead
                Compile(filePath);
                return;
            }
            Zqb zqb = new Zqb();
            {
                using var fs = File.OpenRead(filePath);
                zqb.ReadFrom(fs);
            }

            Dictionary<uint, string> symbolTable = new();
            foreach (var chunk in zqb.Chunks)
            {
                if (chunk is SymbolDef symbolDef)
                {
                    symbolTable[symbolDef.Value] = symbolDef.Name;
                }
            }

            var outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".qcode");
            using var outFs = new FileStream(outputFilePath, FileMode.Create);
            using var writer = new StreamWriter(outFs);
            List<string> tokens = new();
            int indent = 0;
            int nextLineIndentMod = 0;
            foreach (var chunk in zqb.Chunks)
            {
                if (tokens.Count == 0)
                {
                    //only get indent mods from first token
                    var im = chunk.IndentationModifier;
                    if (im.currentLine)
                    {
                        indent = Math.Max(0, indent + im.amount);
                    }
                    else
                    {
                        nextLineIndentMod += im.amount;
                    }
                }

                if (chunk is Symbol symbol)
                {
                    tokens.Add(symbolTable[symbol.Value]);
                }
                else if (chunk is SymbolDef _ || chunk is Terminator __)
                {
                    //dont write out symboldefs and terminator
                }
                else if (chunk is NewLine1 nl)
                {
                    WriteTokens(writer, tokens, indent);
                    indent = Math.Max(0, indent + nextLineIndentMod);
                    nextLineIndentMod = 0;
                    tokens.Clear();
                }
                else
                {
                    tokens.Add(chunk.ToString());
                }
            }
            if (tokens.Count > 0)
            {
                WriteTokens(writer, tokens, indent);
            }
        }

        static void WriteTokens(StreamWriter writer, List<string> tokens, int indent)
        {
            for (int i = 0; i < indent; ++i)
            {
                writer.Write("  ");
            }
            writer.WriteLine(string.Join(' ', tokens));
        }

        static void Compile(string filePath)
        {
        }
    }
}
