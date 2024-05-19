using System;

namespace Obsidian.Persistence;

public class NoteNotFoundException(string id) : Exception($"Note with id '{id}' not found.");