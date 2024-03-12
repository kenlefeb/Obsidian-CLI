// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.CommandLine;

namespace Obsidian.CLI;

/// <summary>
/// Main application class
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point
    /// </summary>
    /// <param name="args">Command Line Parameters</param>
    /// <returns>0 on success</returns>
    public static int Main(string[] args)
    {
        // build the command line args
        Global.RootCommand root = [];

        // an alternate approach to using the ParseResult is to build a
        // middleware handler and inject into the pipeline before the default help handler

        // we all need ascii art :)
        Global.RootCommand.DisplayAsciiArt(root.Parse(args));

        // invoke the correct command handler
        // once you understand what this one line of code does, it's really cool!
        // we add a command handler for each of the leaf commands and this automatically calls that handler
        // no switch or if statements!
        // allows for super clean code with no parsing!
        return root.Invoke(args);
    }
}
