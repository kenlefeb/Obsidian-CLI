using System.IO;

namespace Obsidian.Domain;

public class Note
{
    public Vault Vault { get; set; }
    public FileInfo File { get; set; }
    public string Content { get; set; }
}