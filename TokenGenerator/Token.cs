using TokenGenerator.interfaces;

namespace TokenGenerator;

public class Token : IToken
{
    public string Identifier { get; set; }
    public string Type { get; set; }
    public bool IsDeclaration { get; set; }
    public bool IsComment { get; set; }
    public string? Comment { get; set; }
}