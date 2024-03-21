using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.Persistence
{
    public class DailyNote : Note
    {
        public DateOnly Date { get; set; }
    }

    public class Note
    {
        public string Contents { get; set; }
    }
}
