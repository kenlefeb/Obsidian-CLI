using System;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Linq;
using System.Text.Json;
using Obsidian.CLI.Exceptions;
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
            try
            {
                if (options.DryRun)
                {
                    Console.WriteLine("Daily Note Add Command");
                    Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
                }
                else
                {
                    Vault vault = GetVault(_configuration, options);
                    DailyNote note = CreateNote(vault, options);
                    Console.WriteLine($"Daily Note Created: {note.File.FullName}");
                    if (options.Verbose)
                    {
                        Console.WriteLine(JsonSerializer.Serialize<Options>(options, _configuration.JsonOptions));
                    }
                }

                return 0;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
        }

        private static DailyNote CreateNote(Vault vault, Options options)
        {
            return vault.DailyNotes.Create(options.Date);
        }

        private static Vault GetVault(Global.Configuration configuration, Options options)
        {
            if (configuration.Vaults.Count == 0)
            {
                throw new InvalidConfigurationException("No vaults found in configuration.");
            }
            string id = options.Vault ?? configuration.Vaults[0].Id;
            var vault = configuration.Vaults.FirstOrDefault(v => v.Id == id);
            return vault ?? throw new InvalidConfigurationException($"Vault with id '{id}' not found in configuration.");
        }
    }
}
