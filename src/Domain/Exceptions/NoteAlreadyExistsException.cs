using System;

namespace Obsidian.Persistence;

public class NoteAlreadyExistsException(string id) : Exception($"A note with Id '{id}' already exists.");