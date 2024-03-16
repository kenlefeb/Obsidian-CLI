using System;

namespace Obsidian.CLI.Exceptions;

public class InvalidConfigurationException(string message) : Exception(message);
