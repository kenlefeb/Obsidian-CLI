using System;

namespace Obsidian.CLI.DailyNotes.Add
{
    internal class Options : Configuration.Options
    {
        public bool All { get; set; }
        public string? Vault { get; set; } = null;
        public DateOnly Date { get; set; }
    }
}
