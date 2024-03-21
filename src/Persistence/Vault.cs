using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Obsidian.Domain.Abstractions.Settings;

namespace Obsidian.Persistence
{
    public class Vault
    {
        private readonly ILogger<Vault> _logger;
        private readonly IFileSystem _filesystem;
        private readonly DailyNotes _dailyNotes;

        public Vault(ILogger<Vault> logger, IFileSystem filesystem, IVaultSettings settings, IEnvironmentVariables environment)
        {
            _logger = logger;
            _filesystem = filesystem;
            Environment = environment;
            Settings = settings;
            _dailyNotes = new DailyNotes(this);    // TODO: Replace with a Notes repository that includes DailyNotes
        }

        public bool Exists => _filesystem.Directory.Exists(Settings.Path);

        public string Path => Settings.Path;
        public Domain.Abstractions.Settings.IVaultSettings Settings { get; private set; }
        public IEnvironmentVariables Environment { get; }

        // TODO: Replace Create method with DI container
        public static Vault Create(ILogger<Vault> logger, IFileSystem filesystem, IVaultSettings settings, IEnvironmentVariables environment)
        {
            var vault= new Vault(logger, filesystem, settings, environment);
            if (!vault.Exists)
            {
                filesystem.Directory.CreateDirectory(vault.Path);
            }

            return vault;
        }

        public Note AddDailyNote(DateOnly date, bool force)
        {
            return new DailyNote(this, date);
        }
    }
}
