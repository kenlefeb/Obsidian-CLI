using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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
        return builder.Build().Get<Configuration>();
    }

    private static FileInfo FindConfigurationFile(string name)
    {
        var candidate = System.Environment.ExpandEnvironmentVariables($"%USERPROFILE%\\.obsidian-cli\\{name}");
        return new FileInfo(candidate);
    }
}

public class Vault
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string Path { get; set; }
    public Templates Templates { get; set; } = new Templates();
}

public class Templates
{
    public string Path { get; set; }
    public IList<Template> Items { get; set; } = new List<Template>();
}

public class Template
{
    public string Type { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public string Path { get; set; }
    public Recurrence Recurrence { get; set; } = new EveryDayRecurrence();
    public Template? Extends { get; set; } = null;
}

public class EveryDayRecurrence : Recurrence
{
    public override string Pattern { get; set; } = "*";
}

public class Recurrence
{
    public virtual DateOnly? Start { get; set; } = null;
    public virtual DateOnly? End { get; set; } = null;
    public virtual string Pattern { get; set; } = "*";
}
