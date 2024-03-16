using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Obsidian.Domain.Abstractions.Settings;

namespace Obsidian.Persistence
{
    public class Vault
    {
        private readonly ILogger<Vault> _logger;
        private readonly IFileSystem _filesystem;
        private readonly IVaultSettings _settings;

        public Vault(ILogger<Vault> logger, IFileSystem filesystem, IVaultSettings settings)
        {
            _logger = logger;
            _filesystem = filesystem;
            _settings = settings;
        }

        public bool Exists => _filesystem.Directory.Exists(_settings.Path);

        public string Path => _settings.Path;

        public static Vault Create(ILogger<Vault> logger, IFileSystem filesystem, IVaultSettings settings)
        {
            var vault= new Vault(logger, filesystem, settings);
            if (!vault.Exists)
            {
                filesystem.Directory.CreateDirectory(vault.Path);
            }

            return vault;
        }
    }
}
