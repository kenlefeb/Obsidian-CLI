using System.Collections.Generic;

namespace Obsidian.Domain.Settings;

public class Templates
{
    public string Path { get; set; } = @"library\\templates";
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