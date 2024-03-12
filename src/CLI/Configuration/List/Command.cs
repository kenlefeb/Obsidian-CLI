using System;
using System.CommandLine.NamingConventionBinder;
using System.Text.Json;

namespace Obsidian.CLI.Configuration.List
{
    public class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("list", "List configuration settings.")
        {
            _configuration = configuration;
            AddOption(new AllOption());
            Handler = CommandHandler.Create<Options>(DoCommand);
        }

        public int DoCommand(Options options)
        {
            if (options.DryRun)
            {
                Console.WriteLine("Configuration List Command");
                Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
            }
            else
            {
                Console.WriteLine("Configuration:");
                Console.WriteLine(JsonSerializer.Serialize<Global.Configuration>(_configuration, _configuration.JsonOptions));
            }

            return 0;
        }
    }
}
