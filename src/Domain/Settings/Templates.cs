using System.Collections.Generic;
using Obsidian.Domain.Abstractions.Services;

namespace Obsidian.Domain.Settings;

public class Templates
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