using System.Collections.Generic;

namespace Obsidian.Domain.Settings;

public class Templates
{
    public string Path { get; set; }
    public IList<Template> Items { get; set; } = new List<Template>();
}