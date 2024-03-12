namespace Obsidian.CLI.Configuration
{
    public class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("configuration", "Manipulate the settings for Obsidian CLI.")
        {
            _configuration = configuration;
            // alias because it's easier to type
            AddAlias("config");

            // add the commands to the tree
            AddCommand(new List.Command(configuration));
        }
    }
}
