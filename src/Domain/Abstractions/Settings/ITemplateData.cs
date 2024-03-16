using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.Domain.Abstractions.Settings
{
    public interface ITemplateData
    {
        public DateOnly NoteDate { get; set; }
        public IDictionary<string, string> Environment { get; set; }
    }
}
