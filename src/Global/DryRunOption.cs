using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Global
{
    internal class DryRunOption()
        : Option<bool>(new string[] { "--dry-run", "-d" }, "Validates and displays configuration")
    {
    }
}
