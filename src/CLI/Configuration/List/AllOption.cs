using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Configuration.List
{
    internal class AllOption() : Option<bool>(["--all", "-a"],
        "List all valid settings even if they're not currently set.");
}
