using System;
using System.CommandLine.Parsing;
using System.IO;
using System.Reflection;
using Obsidian.CLI.extensions;

namespace Obsidian.CLI.Global;

internal class RootCommand : System.CommandLine.RootCommand
{
    public RootCommand()
        : base("Obsidian CLI")
    {
        Configuration configuration = Configuration.Load();

        // we use extensions to build each command which makes reuse and reorg really fast and easy
        // notice there is no help or version command added
        // --help -h -? and --version are "automatic"
        // --version is controlled by the semver in the project
        //   versionprefix and versionsuffix

        // add the command handlers
        AddCommand(new Obsidian.CLI.Configuration.Command(configuration));
        AddCommand(new Obsidian.CLI.DailyNotes.Command(configuration));

        // add the global options
        // these options are available to all commands and sub commands
        // see AddBootstrapCommand for additional options on specific commands
        this.AddGlobalOption(new Global.DryRunOption());
        this.AddGlobalOption(new Global.VerboseOption());
    }

    // display Ascii Art
    public static void DisplayAsciiArt(ParseResult parseResult)
    {
        // --version and --help will be parsed as unmatched tokens
        // we use ParseResult extensions to check for the unmatched tokens
        // the default System.CommandLine middleware will handle the unmatched tokens on invoke

        // don't display if --version
        if (!parseResult.HasVersionOption())
        {
            // display for help or dry-run
            if (parseResult.HasHelpOption() || parseResult.HasDryRunOption())
            {
                // you have to get the path for this to work as a dotnet tool
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string file = $"{path}/files/ascii-art.txt";

                try
                {
                    if (File.Exists(file))
                    {
                        string txt = File.ReadAllText(file);

                        if (!string.IsNullOrWhiteSpace(txt))
                        {
                            // GEAUX Tigers!
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine(txt);
                        }
                    }
                }
                catch
                {
                    // ignore any errors
                }
                finally
                {
                    // reset the console
                    Console.ResetColor();
                }
            }
        }
    }
}
