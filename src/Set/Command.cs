using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Obsidian.CLI.Set
{
    internal class Command : System.CommandLine.Command
    {
        public Command()
            : base("set", "example using positional Arguments with validation")
        {
            AddArgument(new KeyArgument());
            AddArgument(new ValueArgument());
            AddValidator(ValidateSet);
            Handler = CommandHandler.Create<AppSetConfig>(DoSetCommand);
        }

        // validate arguments
        private static string ValidateSet(CommandResult result)
        {
            // return non-empty string to display an error
            string msg = string.Empty;

            try
            {
                // get the results
                ArgumentResult keyResult = (ArgumentResult)result.Children.GetByAlias("key");
                ArgumentResult valueResult = (ArgumentResult)result.Children.GetByAlias("value");

                // let System.CommandLine handle this
                if (keyResult == null || valueResult == null)
                {
                    return msg;
                }

                string key = keyResult.GetValueOrDefault<string>();

                // validate the key - stop on error
                if (string.IsNullOrWhiteSpace(key))
                {
                    return msg + "key argument cannot be empty\n  valid keys: {string.Join(' ', ValidKeys).Trim()}\n";
                }
                else
                {
                    // case sensitive compare against list of valid keys
                    if (!KeyArgument.ValidKeys.Contains(key))
                    {
                        return msg + $"invalid key\n  valid keys: {string.Join(' ', KeyArgument.ValidKeys).Trim()}\n";
                    }
                }

                List<string> values = valueResult.GetValueOrDefault<List<string>>();

                // validate values - stop on error
                if (values == null || values.Count == 0)
                {
                    return msg + "Failed to parse value(s)\n";
                }

                // validate the value(s) based on key
                switch (key)
                {
                    case "Port":
                    case "NodePort":
                        // must be integer within range
                        return msg + ValidatePort(key, values);
                    case "Args":
                        // args takes an array of values so skip the default validation
                        break;
                    default:
                        // only one value passed
                        if (values.Count > 1)
                        {
                            return msg + $"{key} only takes one value\n";
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                return msg + $"Parsing exception: {ex.Message}\n";
            }

            return msg;
        }

        // validate Port and NodePort
        //   this is an example where it might be easier to have sub-commands vs. key Argument
        //     System.CommandLine would handle the int parsing by declaration
        private static string ValidatePort(string key, List<string> values)
        {
            string msg = string.Empty;

            // set min and max based on key
            int min = key == "Port" ? 1 : 30000;
            int max = key == "Port" ? 64 * 1024 : 32 * 1024;

            // only one value
            if (values.Count > 1)
            {
                msg += $"{key} only takes one value\n";
            }

            // must be int >= min and < max
            if (!int.TryParse(values[0], out int port) || port < min || port >= max)
            {
                msg += $"{key} must be an integer >= {min} and < {max}\n";
            }

            return msg;
        }

        // set Command Handler
        private static int DoSetCommand(AppSetConfig config)
        {
            if (config.DryRun)
            {
                // handle --dry-run
            }

            // replace with your implementation
            Console.WriteLine("Set Command");
            Console.WriteLine(JsonSerializer.Serialize<AppSetConfig>(config, AppConfig.JsonOptions));

            return 0;
        }

    }
}
