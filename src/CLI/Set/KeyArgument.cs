using System.Collections.Generic;
using System.CommandLine;

namespace Obsidian.CLI.Set
{
    internal class KeyArgument() : Argument<string>("key", $"Value to set ({string.Join(' ', ValidKeys)})")
    {
        // list of valid keys
        //   the format of the command is scl key value [value2 value3 ...]
        //   we could use a sub-command for "key" instead of an argument
        //     with the Argument approach, we have to do some parsing and a switch for validation
        //     with the sub-command approach, you have one handler per leaf command
        //     the number and type of keys and the complexity of validation seem to drive this choice
        public static IEnumerable<string> ValidKeys =>
        [
            "Namespace",
            "AppName",
            "Args",
            "Port",
            "NodePort",
        ];
    }
}
