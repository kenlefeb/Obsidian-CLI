namespace Obsidian.Domain.Settings
{
    public class VaultSettings
    {
        public DailyNotes DailyNotes { get; set; } = new DailyNotes();
        public Templates Templates { get; set; } = new Templates();
    }
}
