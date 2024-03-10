using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Linq;

namespace Obsidian.CLI.Configuration
{
    public class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("configuration", "Manipulate the settings for Obsidian CLI.")
        {
            _configuration = configuration;
            // alias because it's easier to type
            AddAlias("config");

            // add the commands to the tree
            AddCommand(new Set.Command());
            AddCommand(new Unset.Command());
            AddCommand(new List.Command(configuration));
        }

        // validate the command parameters
        internal static void ValidateCommand(CommandResult result)
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
