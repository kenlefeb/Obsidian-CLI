using System.Collections.Generic;
using System.CommandLine;

namespace Obsidian.CLI.Set
{
    internal class ValueArgument() : Argument<List<string>>("value", "New value(s)");
}
