namespace TokenGenerator;

public class TypescriptToken : Token
{
    public bool IsOptional;
    public bool IsNull;
    public bool IsDate;
    public bool IsArray;
    public bool IsDictionary;
    public bool Skip;
}
