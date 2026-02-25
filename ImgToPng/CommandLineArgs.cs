using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgToPng
{
    public class CommandLineArgs
    {
        [Value(0, MetaName = "Input File (img)", Required = true, HelpText = "the img file to convert")]
        public string InputFile { get; set; } = string.Empty;
    }
}
