using TokenGenerator.Handlers.Type;
using TokenGenerator.interfaces;

namespace TokenGenerator;

public class TypescriptToken : Token, IParsedToken
{
    public bool IsOptional { get; set; }
    public bool IsNull { get; set; }
    public bool IsDate { get; set; }
    public bool IsArray { get; set; }
    public bool IsDictionary { get; set; }
    public bool IsCustomType { get; set; }
    public HashSet<SkipOptions> Skip { get; set; } = new();
    public string[]? CustomTypes { get; set; }
}