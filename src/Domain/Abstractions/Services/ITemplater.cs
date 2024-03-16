namespace Obsidian.Domain.Abstractions.Services;

public interface ITemplater
{
    string Render(string template, object? data = null);
}