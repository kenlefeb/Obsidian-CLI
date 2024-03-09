using System.Collections.Generic;
using System.Linq;
using System.CommandLine.Parsing;

namespace Obsidian.CLI.Bootstrap
{
    public class Command : System.CommandLine.Command
    {
        public Command()
            : base("bootstrap", "example of using sub-command specific options and validation")
        {
            // alias because it's easier to type
            AddAlias("bs");

            // add the commands to the tree
            AddCommand(new Add.Command());
            AddCommand(new Remove.Command());
        }

        // validate the bootstrap command parameters
        internal static void ValidateBootstrapCommand(CommandResult result)
        {
            string msg = string.Empty;

            try
            {
                // get the values to validate
                List<string> services = result.Children.FirstOrDefault(c => c.Symbol.Name == "services") is OptionResult svcRes ? svcRes.GetValueOrDefault<List<string>>() : null;
                bool all = result.Children.FirstOrDefault(c => c.Symbol.Name == "all") is OptionResult allRes && allRes.GetValueOrDefault<bool>();

                if (services != null && services.Count == 0)
                {
                    services = null;
                }

                // must specify --services or --all
                if (!all && services == null)
                {
                    msg += "--services or --all must be specified";
                }

                // can't specify both
                if (all && services != null)
                {
                    msg += "--services and --all cannot be combined";
                }
            }
            catch
            {
                // system.commandline will catch and display parse exceptions
            }

            // return error message(s) or string.empty
            result.ErrorMessage = msg;
        }

    }
}
