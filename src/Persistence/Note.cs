using System.IO;

namespace Obsidian.Persistence;

public class Note
{
    protected Note(Vault vault, string path) : this(vault)
    {
        File = new FileInfo(path);
    }

    protected Note(Vault vault)
    {
        Vault = vault;
    }

    public Vault Vault { get; private set; }
    public FileInfo File { get; protected set; }

    public string Contents { get; set; }
}