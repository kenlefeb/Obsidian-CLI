using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Obsidian.Domain;

namespace Obsidian.CLI.Global;

public class Configuration
{
    /// <summary>
    /// Gets or sets the JSON serialization options
    /// </summary>
    public JsonSerializerOptions JsonOptions { get; set; } = new () { WriteIndented = true };

    public IList<Vault> Vaults { get; set; } = new List<Vault>();

    public static Configuration Load()
    {
        FileInfo file = FindConfigurationFile("settings.json");
        var builder = new ConfigurationBuilder();
        if (file?.Exists ?? false)
            builder.AddJsonFile(file.FullName);
        else
            builder.AddJsonFile(file.Name);
        builder.AddEnvironmentVariables();
        builder.AddUserSecrets<Configuration>(optional: true);
        var configuration = builder.Build();
        return configuration?.Get<Configuration>() ?? new Configuration();
    }

    private static FileInfo FindConfigurationFile(string name)
    {
        var candidate = System.Environment.ExpandEnvironmentVariables($"%USERPROFILE%\\.obsidian-cli\\{name}");
        return new FileInfo(candidate);
    }
}

