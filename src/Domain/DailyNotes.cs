using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Obsidian.Persistence;

namespace Obsidian.Domain;

public class DailyNotes : IQueryable<DailyNote>
{
    private readonly Vault _vault;
    private readonly EnvironmentVariables _environment;
    private readonly Lazy<IQueryable<DailyNote>> _notes;

    public DailyNotes(Vault vault, EnvironmentVariables environment)
    {
        _vault = vault;
        _environment = environment;
    }


    public IEnumerator<DailyNote> GetEnumerator()
    {
        return _notes.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_notes.Value).GetEnumerator();
    }

    public Type ElementType => _notes.Value.ElementType;

    public Expression Expression => _notes.Value.Expression;

    public IQueryProvider Provider => _notes.Value.Provider;
}