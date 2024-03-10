using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using Obsidian.CLI.extensions;
using Obsidian.CLI.Extensions;
using Obsidian.CLI.model;

namespace Obsidian.CLI.Build
{
    internal class Command : System.CommandLine.Command
    {
        public Command()
            : base("build", "example using an enum option with defaults")
        {
            Handler = CommandHandler.Create<BuildConfig>(CommandHandlers.DoBuildCommand);
            AddOption(new BuildTypeOption());
        }
    }
}
