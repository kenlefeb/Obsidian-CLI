using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Obsidian.Domain;

namespace Obsidian.CLI.Global;

public class Configuration
{
    /// <summary>
    /// Gets or sets the JSON serialization options
    /// </summary>
    public JsonSerializerOptions JsonOptions { get; set; } = new() { WriteIndented = true };

    public IList<Vault> Vaults { get; set; } = [];

    public static Configuration Load()
    {
        ConfigurationBuilder builder = new();
        _ = builder.AddJsonFile("settings.json");
        _ = builder.AddEnvironmentVariables();
        _ = builder.AddUserSecrets<Configuration>(optional: true);
        IConfigurationRoot configuration = builder.Build();
        return configuration?.Get<Configuration>() ?? new Configuration();
    }
}

