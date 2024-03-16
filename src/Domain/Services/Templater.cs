using System;
using System.Collections.Generic;
using Fluid;
using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Abstractions.Settings;
using Obsidian.Persistence;

namespace Obsidian.Domain.Services
{
    public class Templater(ITemplateData data) : ITemplater
    {
        public string Render(string template, object? data1 = null)
        {
            var parser = new FluidParser();

            if (parser.TryParse(template, out var parsed, out var error))
            {
                var context = new TemplateContext(data1 ?? data);
                return parsed.Render(context);
            }

            return $"Error: {error}";
        }
    }

}
