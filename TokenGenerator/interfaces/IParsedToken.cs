using TokenGenerator.Handlers.Type;

namespace TokenGenerator.interfaces;

public interface IParsedToken : IToken
{
    public bool IsOptional { get; set; }
    public bool IsNull { get; set; }
    public bool IsDate { get; set; }
    public bool IsArray { get; set; }
    public bool IsDictionary { get; set; }
    public HashSet<SkipOptions> Skip { get; set; }
    public string[]? CustomTypes { get; set; }
    public bool IsCustomType { get; set; }
}