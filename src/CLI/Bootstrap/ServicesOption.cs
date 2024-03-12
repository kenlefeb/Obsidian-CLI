using System.Collections.Generic;
using System.CommandLine;

namespace Obsidian.CLI.Bootstrap;

internal class ServicesOption() : Option<List<string>>(new[] { "--services", "-s" }, "array of string(s)");
