using System.CommandLine.Invocation;
using Obsidian.CLI.CommandLine.Extensions;

namespace Obsidian.CLI.Bootstrap.Add
{
    public class Command : System.CommandLine.Command
    {
        public Command() : base("add", "example of using sub-command specific options and validation")
        {
            Handler = CommandHandler.Create<BootstrapConfig>(CommandHandlers.DoBootstrapAddCommand);

            // these options will only be available to this command
            //   we could add as global options to the parent command
            //   we chose to do it this way to get the custom description
            // Note that our handler uses a different model for the command
            AddOption(new ServicesOption());
            AddOption(new AllOption());

            // command validator to make sure -s or -a (but not both) are provided
            AddValidator(Bootstrap.Command.ValidateBootstrapCommand);

        }
    }
}
