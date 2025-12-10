namespace TokenGenerator.interfaces;

public interface IToken
{
    public string Identifier { get; set; }
    public string Type { get; set; }
    public bool IsDeclaration { get; set; }
    public bool IsComment { get; set; }
    public string? Comment { get; set; }
}