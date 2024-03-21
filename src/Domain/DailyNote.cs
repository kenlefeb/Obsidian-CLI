
using Obsidian.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Obsidian.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Obsidian.Domain;

public class DailyNote : Note
{

    public DailyNote(Vault vault, DateOnly date)
    {
    }

    public DailyNote(Vault vault, string path)
    {
    }
}