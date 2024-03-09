using Obsidian.CLI.CommandLine.Extensions;
using System.CommandLine;
using System.CommandLine.Invocation;

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
