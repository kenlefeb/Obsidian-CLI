using Obsidian.Domain.Settings;
using Obsidian.Persistence;

namespace Obsidian.Domain
{
    public class Vault
    {
        public Vault() : this(new EnvironmentVariables()) { }

        public Vault(EnvironmentVariables environment)
        {
            DailyNotes = new DailyNotes(this, environment);

        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public VaultSettings Settings { get; set; } = new VaultSettings();
        public DailyNotes DailyNotes { get; set; }
    }
}
