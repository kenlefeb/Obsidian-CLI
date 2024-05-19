using System.IO;
using System.IO.Abstractions;

namespace Obsidian.Persistence;

public class Note
{
    public Note(Vault vault, string path) : this(vault)
    {
        File = vault.GetFile(path);
    }

    protected Note(Vault vault)
    {
        Vault = vault;
    }

    public Vault Vault { get; private set; }
    public IFileInfo File { get; protected set; }

    public string Contents { get; set; }
    public string Id => File.FullName;

    public override bool Equals(object obj)
    {
        if (obj is Note otherNote)
        {
            return this.File.FullName.Equals(otherNote.File.FullName);
        }
        return false;
    }
    public override int GetHashCode()
    {
        return this.File.FullName.GetHashCode();
    }
}