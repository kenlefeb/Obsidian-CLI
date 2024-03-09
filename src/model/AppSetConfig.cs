// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Obsidian.CLI;

/// <summary>
/// Model for setting app values
/// System.CommandLine will parse and pass to the handler
/// </summary>
public class AppSetConfig : AppConfig
{
    /// <summary>
    /// Gets or sets a value indicating the key for the new value
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the new value for the command
    /// </summary>
    public List<string> Value { get; set; }
}
