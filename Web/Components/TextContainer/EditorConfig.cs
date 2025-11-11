namespace Web.Components.TextContainer;

public sealed class EditorConfig(
    bool isReadOnly,
    string title,
    string logoPath = "")
{
    public bool IsReadOnly { get; } = isReadOnly;
    public string Title { get; } = title;
    public string? LogoPath { get; set; } = logoPath;
}

