using Parser.Context;
using Parser.Token;
using Parser.utils;

namespace Parser.TokenGenerator;

public class TypescriptTokenGenerator : ITokenGenerator
{
    private readonly Dictionary<string, string> types = new()
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
        { "object", "any" }
    };

    public TypescriptToken InterpretToken(Token.Token token)
    {
        var _t = new TypescriptToken
        {
            Type = GetType(token),
            Identifier = token.Identifier!,
            IsNullable = IsNullable(token),
            IsArray = IsArray(token),
            IsDictionary = IsDictionary(token)
            
        };

        FormatIfArray(_t);
        FormatIfNullable(_t);
        FormatIfDictionary(_t);

        return _t;
    }

    private string GetType(Token.Token token)
    {
        var tokenType = token.Type!;
        tokenType = tokenType.Replace("[]", "").Replace("?","");
        return types.GetValueOrDefault(tokenType.ToLower(), tokenType);
    }

    private static bool IsNullable(Token.Token token)
    {
        return token.Type!.Contains('?') && InputValidator.ValidateNullableFormat(token.Type);
    }

    private static bool IsArray(Token.Token token)
    {
        if (token.Type!.Contains("[]") && !token.IsTypeDeclaration)
            return InputValidator.ValidateArrayFormat(token.Type);

        return token.Type!.Contains("List") && InputValidator.ValidateListFormat(token.Type);
    }

    private static bool IsDictionary(Token.Token token)
    {
        return token.Type!.Contains("Dictionary") && InputValidator.ValidateDictionaryFormat(token.Type);
    }

    private static void FormatIfNullable(TypescriptToken token)
    {
        if (!token.IsNullable)
            return;
        
        token.Type = token.Type.Replace("?", "");
        token.Identifier = $"{token.Identifier}?";
    }


    private static void FormatIfArray(TypescriptToken token)
    {
        if (!token.IsArray) 
            return;
        
        var type = token.Type.Replace("List", "").Replace("<", "").Replace(">", "").Replace("[]","");
        token.Type = $"{type}[]";
    }

    private static void FormatIfDictionary(TypescriptToken token)
    {
        if (!token.IsDictionary) 
            return;
        
        Console.WriteLine(token.Type);
        
        string types = token.Type.Replace("Dictionary", "").Replace("<", "").Replace(">", "");
        string type1 = types.Split(",")[0];
        string type2 = types.Split(",")[1];
        token.Type = $"Record<{type1},{type2}>";
    }
    
    
    
}