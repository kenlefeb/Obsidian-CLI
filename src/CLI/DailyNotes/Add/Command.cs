using System;
using System.CommandLine.NamingConventionBinder;
using System.Text.Json;

namespace Obsidian.CLI.DailyNotes.Add
{
    public class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("add", "Create a new Daily Note.")
        {
            _configuration = configuration;
            Handler = CommandHandler.Create<Options>(DoCommand);
        }

        public int DoCommand(Options options)
        {
            if (options.DryRun)
            {
                Console.WriteLine("Daily Note Add Command");
                Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
            }
            else
            {
                var note = new DailyNote(_configuration, options);
                Console.WriteLine($"Daily Note Created: {note.File.FullName}");
                if (options.Verbose)
                {
                    Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
                }
            }

            return 0;
        }
    }
}
