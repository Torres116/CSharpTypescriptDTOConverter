namespace TokenGenerator;

public class TypescriptToken : Token
{
    public bool IsOptional = false;
    public bool IsNull = false;
    public bool IsDate = false;
    public bool IsArray = false;
    public bool IsDictionary = false;
    public bool Skip = false;
    public bool IsConstructor = false;
    public bool IsConstructorParameter = false;
}
