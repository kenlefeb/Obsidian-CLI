using Fluid;
using Obsidian.Domain.Abstractions.Services;

namespace Obsidian.Domain.Services
{
    public class Templater(TemplateData data) : ITemplater
    {
        public Templater() : this(new TemplateData())
        {
        }

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
