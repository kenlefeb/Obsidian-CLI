using System.CommandLine;
using Obsidian.CLI.model;

namespace Obsidian.CLI.Build
{
    internal class BuildTypeOption()
        : Option<BuildType>(new string[] { "--build-type", "-b" }, () => BuildType.Debug, "Build type");
}
