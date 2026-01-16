using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToTex
{
    public class CommandLineArgs
    {
        [Value(0, MetaName = "Input Image Files", Required = true, HelpText = "Any number of image files in all supported formats (using ImageSharp)")]
        public IEnumerable<string> InputFiles { get; set; } = Enumerable.Empty<string>();

        [Option("output", Required = true, HelpText = "the path to the output file")]
        public string OutputFile { get; set; } = string.Empty;
    }
}
