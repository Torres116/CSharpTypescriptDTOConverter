namespace TokenGenerator;

public class TypescriptToken : Token
{
    public bool IsOptional = false;
    public bool IsNull = false;
    public bool IsDate = false;
    public bool IsArray = false;
    public bool IsDictionary = false;
    public bool Skip = false;
    public bool TokenCustomTypeSkip = false;
    public string[]? CustomTypes;
    public bool SkipDictionary = false;
}
