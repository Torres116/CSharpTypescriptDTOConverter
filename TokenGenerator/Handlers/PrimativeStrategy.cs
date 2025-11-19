namespace TokenGenerator.Handlers;

public sealed class PrimitiveTypeMapper
{
    private static readonly Dictionary<string, string> Types = new()
    {
        { "string", "string" },
        { "guid", "string" },
        { "bool", "boolean" },
        { "datetime", "Date" },
        { "timespan", "Date" },
        { "int", "number" },
        { "float", "number" },
        { "double", "number" },
        { "long", "number" },
        { "short", "number" },
        { "decimal", "number" },
        { "byte", "number" },
        { "sbyte", "number" },
        { "uint", "number" },
        { "ulong", "number" },
        { "ushort", "number" },
        { "object", "any" },
    };

    public static void Convert(TypescriptToken token)
    {
        if (token.Type == null)
            return;
        
        var type = token.Type?.Replace("?", "").Replace("[]","").ToLower();
        var result = Types!.GetValueOrDefault(type,null);
        
        if (result != null)
        {
            if(token.Type!.Contains("[]"))
                result += "[]";
            
            if(token.Type.Contains('?'))
                result += '?';
            
            token.Type = result;
        } 
        
    }
}