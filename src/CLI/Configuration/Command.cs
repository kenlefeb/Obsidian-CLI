namespace Obsidian.CLI.Configuration
{
    public class Command : System.CommandLine.Command
    {
        public Command(Global.Configuration configuration)
            : base("configuration", "Manipulate the settings for Obsidian CLI.")
        {
            // alias because it's easier to type
            AddAlias("config");

            // add the commands to the tree
            AddCommand(new List.Command(configuration));
        }
    }
}
