namespace Parser.Token;

public class TypescriptToken
{
    public string Identifier { get; set; }
    public string Type { get; set; }
    public bool IsArray { get; set; }
    public bool IsDictionary { get; set; }
    public bool IsNullable { get; set; }
}
