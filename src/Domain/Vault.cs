using System.Collections.Generic;
using System;

namespace Obsidian.Domain
{
    public class Vault
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public Settings Settings { get; set; } = new Settings();
    }

    public class Settings
    {
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
}
