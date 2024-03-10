using System;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Text.Json;
using Obsidian.Domain;

namespace Obsidian.CLI.DailyNotes.Add
{
    public class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("add", "Create a new Daily Note.")
        {
            _configuration = configuration;
            AddOption(new DateOption());
            AddOption(new VaultOption());
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
                var vault = GetVault(_configuration, options);
                var note = CreateNote(vault, options);
                Console.WriteLine($"Daily Note Created: {note.File.FullName}");
                if (options.Verbose)
                {
                    Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
                }
            }

            return 0;
        }

        private DailyNote CreateNote(Vault vault, Options options)
        {
            return vault.DailyNotes.Create(options.Date);
        }

        private Vault GetVault(Global.Configuration configuration, Options options)
        {
            var id = options.Vault ?? configuration.Vaults[0].Id;
            return configuration.Vaults.First(v => v.Id == id);
        }
    }
}
