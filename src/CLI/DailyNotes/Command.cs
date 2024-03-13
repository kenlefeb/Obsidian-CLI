namespace Obsidian.CLI.DailyNotes
{
    internal class Command : System.CommandLine.Command
    {
        private readonly Global.Configuration _configuration;

        public Command(Global.Configuration configuration)
            : base("daily-note", "Work with a Daily Note.")
        {
            _configuration = configuration;
            AddAlias("dn");
            AddAlias("daily");

            AddCommand(new Add.Command(configuration));
        }
    }
}
