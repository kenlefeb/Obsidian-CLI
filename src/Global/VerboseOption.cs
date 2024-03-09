using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Global
{
    internal class VerboseOption()
        : Option<bool>(new string[] { "--verbose", "-v" }, "Show verbose output")
    {
    }
}
