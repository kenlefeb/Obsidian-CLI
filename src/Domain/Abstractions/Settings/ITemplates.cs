using System.Collections.Generic;

namespace Obsidian.Domain.Abstractions.Settings;

public interface ITemplates
{
    string Path { get; set; }
    IList<Template> Items { get; set; }
}