using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Set
{
    internal class ValueArgument() : Argument<List<string>>("value", "New value(s)");
}
