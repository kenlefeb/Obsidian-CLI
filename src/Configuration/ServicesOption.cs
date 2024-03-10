using System.Collections.Generic;
using System.CommandLine;

namespace Obsidian.CLI.Configuration
{
    internal class ServicesOption() : Option<List<string>>(new string[] { "--services", "-s" }, "array of string(s)");
}
