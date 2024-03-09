using System.CommandLine.Invocation;
using Obsidian.CLI.CommandLine.Extensions;

namespace Obsidian.CLI.Add
{
    public class Command : System.CommandLine.Command
    {
        public Command() : base("add", "example of using environment variables as default values")
        {
            // note we are using UserConfig so we can pick up the option we add below
            this.Handler = CommandHandler.Create<UserConfig>(CommandHandlers.DoAddCommand);

            // loading from env var using the EnvVarOptions extension
            //   this will pickup the user from the env vars
            //     you can override by specifying on the command line
            //   by using aliases, we can support all 3 options
            //     --user works for Linux / Mac env var
            //     --username works for Windows env var
            //     -u is the short option for convenience
            // imagine the code you didn't have to write ...
            this.AddOption(EnvVarOptions.AddOption(new string[] { "--user", "--username", "-u" }, "User name", string.Empty));

        }
    }
}
