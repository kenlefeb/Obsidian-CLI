// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Text.Json;
using Obsidian.CLI.model;

namespace Obsidian.CLI.Extensions;

/// <summary>
/// Command handlers for each leaf command
/// These are very simple examples
/// The way System.CommandLine handles calling the right handler is simple and elegant
///   which allows you to write very clean code and avoid parsing
/// Another structure option is to include the Add extension and the handler in a "command class"
///   that is how we initially implemented it and we decided we liked the "grouping" better
///   depending on how complex your handlers are, you may want to use a different approach
/// Grouping the Add extension, [validation] and handler in a command class also provides some interesting reuse capabilities
/// </summary>
public static class CommandHandlers
{
    /// <summary>
    /// add Command Handler
    ///   this uses the UserConfig which supports the --user option
    /// </summary>
    /// <param name="config">parsed command line in UserConfig</param>
    /// <returns>0 on success</returns>
    public static int DoAddCommand(UserConfig config)
    {
        // note we use UserConfig instead of AppConfig
        // we added the --user --username -u option in the Add extension
        // this will pickup the user from the env vars
        // you can override by specifying on the command line
        // --user works for Linux / Mac
        // --username works for Windows
        // by using aliases, we can support all 3 options

        if (config.DryRun)
        {
            // handle --dry-run
        }

        // replace with your implementation
        Console.WriteLine("Add Command");
        Console.WriteLine(JsonSerializer.Serialize<UserConfig>(config, AppConfig.JsonOptions));

        return 0;
    }

    /// <summary>
    /// bootstrap-add Command Handler
    ///   this uses the BootstrapConfig which requires --all or --services
    /// </summary>
    /// <param name="config">parsed command line in BootstrapConfig</param>
    /// <returns>0 on success</returns>
    public static int DoBootstrapAddCommand(BootstrapConfig config)
    {
        if (config.DryRun)
        {
            // handle --dry-run
        }

        // replace with your implementation
        Console.WriteLine("Bootstrap Add Command");
        Console.WriteLine(JsonSerializer.Serialize<BootstrapConfig>(config, AppConfig.JsonOptions));

        return 0;
    }

    /// <summary>
    /// bootstrap-remove Command Handler
    ///   this uses the BootstrapConfig which requires --all or --services
    /// </summary>
    /// <param name="config">parsed command line in BootstrapConfig</param>
    /// <returns>0 on success</returns>
    public static int DoBootstrapRemoveCommand(BootstrapConfig config)
    {
        if (config.DryRun)
        {
            // handle --dry-run
        }

        // replace with your implementation
        Console.WriteLine("Bootstrap Remove Command");
        Console.WriteLine(JsonSerializer.Serialize<BootstrapConfig>(config, AppConfig.JsonOptions));

        return 0;
    }

    /// <summary>
    /// build Command Handler
    ///   this uses BuildConfig to support the BuildType enum
    /// </summary>
    /// <param name="config">parsed command line in AppConfig</param>
    /// <returns>0 on success</returns>
    public static int DoBuildCommand(BuildConfig config)
    {
        if (config.DryRun)
        {
            // handle --dry-run
        }

        // replace with your implementation
        Console.WriteLine("Build Command");
        Console.WriteLine(JsonSerializer.Serialize<BuildConfig>(config, AppConfig.JsonOptions));

        return 0;
    }
}
