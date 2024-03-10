// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.CommandLine.Parsing;
using System.Linq;

namespace Obsidian.CLI.extensions;

/// <summary>
/// Extension methods to check ParseResult for specific unmatched tokens
/// </summary>
public static class ParseResultExtensions
{
    /// <summary>
    /// Extension method to check for --version
    /// </summary>
    /// <param name="parseResult">System.CommandLine.ParseResult</param>
    /// <returns>has unmatched token</returns>
    public static bool HasVersionOption(this ParseResult parseResult)
    {
        return parseResult.UnmatchedTokens.Contains("--version");
    }

    /// <summary>
    /// Extension method to check for help tokens
    /// </summary>
    /// <param name="parseResult">System.CommandLine.ParseResult</param>
    /// <returns>has unmatched token</returns>
    public static bool HasHelpOption(this ParseResult parseResult)
    {
        return parseResult.UnmatchedTokens.Contains("--help") ||
               parseResult.UnmatchedTokens.Contains("-h") ||
               parseResult.UnmatchedTokens.Contains("-?");
    }

    /// <summary>
    /// Extension method to check for dry run tokens
    /// </summary>
    /// <param name="parseResult">System.CommandLine.ParseResult</param>
    /// <returns>has unmatched token</returns>
    public static bool HasDryRunOption(this ParseResult parseResult)
    {
        return parseResult.UnmatchedTokens.Contains("--dry-run") || parseResult.UnmatchedTokens.Contains("-d");
    }
}
