using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToImg
{
    public class CommandLineArgs
    {
        [Value(0, MetaName = "Input File (Any Image)", Required = true, HelpText = "the image file to convert to img")]
        public string InputFile { get; set; } = string.Empty;

        [Option("usepalette", Required = false, Default = false, HelpText = "use palette for the image if possible")]
        public bool UsePalette { get; set; }
    }
}
