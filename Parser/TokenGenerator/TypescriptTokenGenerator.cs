using Parser.Context;
using Parser.Token;
using Parser.utils;

namespace Parser.TokenGenerator;

/// <summary>
/// A generator class responsible for converting parsed tokens into their TypeScript equivalent.
/// </summary>
/// <remarks>
/// This class implements the ITokenGenerator interface and provides functionalities
/// to interpret and transform token objects into a TypescriptToken structure. The
/// transformation accounts for properties such as type, identifier, nullable state,
/// and array state.
/// </remarks>
public class TypescriptTokenGenerator : ITokenGenerator
{
    /// A dictionary mapping C# data types to their equivalent TypeScript types.
    /// Key represents the C# type, and the corresponding value represents the TypeScript type.
    /// Supported mappings include primitive types, collections, and other standard types.
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

    /// Transforms a given token into a corresponding TypescriptToken representation.
    /// <param name="token">The token to be converted, containing relevant type and identifier information.</param>
    /// <returns>A newly created instance of TypescriptToken, representing the provided token with Typescript-specific formatting.</returns>
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

    /// Retrieves the type of the given token based on its type definition and maps it to a corresponding TypeScript type if applicable.
    /// <returns>The mapped TypeScript type as a string if found; otherwise, the original token type.</returns>
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

    /// Determines whether a given token represents an array or a list in its type definition.
    /// <returns>True if the token type represents an array or a list; otherwise, false.</returns>
    private static bool IsArray(Token.Token token)
    {
        if (token.Type!.Contains("[]") && !token.isTypeDeclaration)
            return InputValidator.ValidateArrayFormat(token.Type);

        return token.Type!.Contains("List") && InputValidator.ValidateListFormat(token.Type);
    }

    /// Determines whether the type of the given token represents a dictionary structure.
    /// <returns>A boolean value indicating whether the token's type corresponds to a dictionary format.</returns>
    private static bool IsDictionary(Token.Token token)
    {
        return token.Type!.Contains("Dictionary") && InputValidator.ValidateDictionaryFormat(token.Type);
    }

    /// Adjusts the Type and Identifier properties of the given TypescriptToken if it is marked as nullable.
    private static void FormatIfNullable(TypescriptToken token)
    {
        if (!token.IsNullable)
            return;
        
        token.Type = token.Type.Replace("?", "");
        token.Identifier = $"{token.Identifier}?";
    }

    /// Modifies the type of the provided token to reflect an array data structure if it represents one.
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
        
        string types = token.Type.Replace("Dictionary", "").Replace("<", "").Replace(">", "");
        string type1 = types.Split(",")[0];
        string type2 = types.Split(",")[1];
        token.Type = $"Record<{type1}, {type2}>";
    }
    
    
    
}