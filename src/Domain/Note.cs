namespace Obsidian.Domain;

public class Note
{
    private string _title;

    public string Id { get; set; }

    public string Title
    {
        get
        {
            if (string.IsNullOrEmpty(_title))
                _title = Id;
            return _title;
        }
        set => _title = value;
    }

    public FrontMatter FrontMatter { get; set; } = new FrontMatter();

    public string Contents { get; set; }
}