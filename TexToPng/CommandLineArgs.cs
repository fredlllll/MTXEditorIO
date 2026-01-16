using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TexToPng
{
    public class CommandLineArgs
    {
        [Option("dumpmips", Required = false, Default = false, HelpText = "also dump mip maps of textures")]
        public bool DumpMips { get; set; }

        [Value(0, MetaName = "Input File (Ztex)", Required = true, HelpText = "the tex file from the pc version of the game, usually ends in Ztex")]
        public string InputFile { get; set; } = string.Empty;
    }
}
