﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsidian.Domain.Abstractions.Settings;
using Obsidian.Persistence;

namespace Obsidian.Domain.Services
{
    public class TemplateData : ITemplateData
    {
        public DateOnly NoteDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public IDictionary<string, string> Environment { get; set; } = new EnvironmentVariables();
    }
}
