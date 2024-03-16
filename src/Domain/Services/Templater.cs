using System;
using System.Collections.Generic;
using Fluid;
using Obsidian.Persistence;

namespace Obsidian.Domain.Services
{
    public class Templater
    {
        public string Render(string template, object data)
        {
            var parser = new FluidParser();
            

            if (parser.TryParse(template, out var parsed, out var error))
            {
                var context = new TemplateContext(data);
                return parsed.Render(context);
            }

            return $"Error: {error}";
        }
    }

    public struct TemplateData
    {
        private EnvironmentVariables _environment;
        public DateOnly NoteDate { get; set; }

        public EnvironmentVariables Environment
        {
            get { return _environment ??= new EnvironmentVariables(); }
            set => _environment = value;
        }
    }
}
