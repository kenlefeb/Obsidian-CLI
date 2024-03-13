namespace Obsidian.CLI.DailyNotes
{
    internal class Command : System.CommandLine.Command
    {
        public Command(Global.Configuration configuration)
            : base("daily-note", "Work with a Daily Note.")
        {
            AddAlias("dn");
            AddAlias("daily");

            AddCommand(new Add.Command(configuration));
        }
    }
}
