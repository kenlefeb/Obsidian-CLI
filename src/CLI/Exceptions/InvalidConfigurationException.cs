using System;

namespace Obsidian.CLI.Exceptions;

internal class InvalidConfigurationException(string message) : Exception(message);
