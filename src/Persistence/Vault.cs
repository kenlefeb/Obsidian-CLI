using Microsoft.Extensions.Logging;

using Obsidian.Domain.Abstractions;
using Obsidian.Domain.Settings;

using System.IO.Abstractions;

namespace Obsidian.Persistence
{
    public class Vault(ILogger<Vault> logger, IFileSystem filesystem, VaultSettings settings)
        : IVault
    {
        private readonly ILogger<Vault> _logger = logger;
    }
}
