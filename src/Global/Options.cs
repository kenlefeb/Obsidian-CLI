using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Global
{
    public class Options
    {
        /// <summary>
        /// Gets or sets a value indicating whether this is a dry run
        /// </summary>
        public bool DryRun { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to provide verbose logging
        /// </summary>
        public bool Verbose { get; set; }

    }
}
