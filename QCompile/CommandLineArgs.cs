using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QCompile
{
    public class CommandLineArgs
    {
        [Option("compile", Required = false, Default = false, HelpText = "compile the file")]
        public bool Compile { get; set; }

        [Value(0, MetaName = "Input File (Q File or code file)", Required = true, HelpText = "the file that is supposed to be processed")]
        public string InputFile { get; set; } = string.Empty;
    }
}
