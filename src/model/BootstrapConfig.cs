// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Obsidian.CLI;

/// <summary>
/// Model for bootstrap commands
/// System.CommandLine will parse and pass to the handler
/// Repeat this pattern for custom command options
/// </summary>
public sealed class BootstrapConfig : AppConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether to apply to all services
    /// </summary>
    public bool All { get; set; }

    /// <summary>
    /// Gets or sets a list of services to apply to
    /// </summary>
    public List<string> Services { get; set; }
}
