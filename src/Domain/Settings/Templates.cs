using System.Collections.Generic;
using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Abstractions.Settings;

namespace Obsidian.Domain.Settings;

public class Templates : ITemplates
{

    public string Path { get; set; }

    public IList<Template> Items { get; set; } = new List<Template>{
        new Template
        {
            Type = "Daily Note",
            Name = "Default",
            IsDefault = true,
            Recurrence = new EveryDayRecurrence(),
            Extends = null
        }
    };
}