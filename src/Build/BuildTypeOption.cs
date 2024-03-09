using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.Build
{
    internal class BuildTypeOption()
        : Option<BuildType>(new string[] { "--build-type", "-b" }, () => BuildType.Debug, "Build type");
}
