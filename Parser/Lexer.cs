using System.Text.RegularExpressions;
using TokenGenerator;

namespace Parser;

internal sealed class Lexer
{
    readonly string[] ignoredKeywords =
    [
        "public",
        "private",
        "protected",
        "internal",
        "async"
    ];

    readonly string[] _declarationKeywords =
    [
        "class",
        "record"
    ];

    readonly string[] _tokenType =
    [
        "class",
        "interface",
        "record",
        "string",
        "bool",
        "int",
        "float",
        "double",
        "char",
        "datetime",
        "timespan"
    ];

    enum TypeDeclaration
    {
        CLASS,
        RECORD,
        NONE
    }

    public  List<Token> Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        var result = new List<Token>();
        var separators = new[] { "\n" };
        var formattedInput = input
            .Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();

        // Remove generic whitespaces
        const string genericTypePattern = @"(?<=<[^>]*)\s+(?=[^<]*>)";
        for (var k = 0; k < formattedInput.Length; k++)
            formattedInput[k] = Regex.Replace(formattedInput[k], genericTypePattern, "");

        var typeDeclaration = TypeDeclaration.NONE;

        try
        {
            var declaration = formattedInput[0].Split([" "], StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < declaration.Length; index++)
            {
                var current = declaration[index].Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "");
                var next = declaration[index + 1].Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "");
                if (_declarationKeywords.Contains(current.ToLower()))
                {
                    Token token = new()
                    {
                        Type = current,
                        Identifier = next,
                        IsDeclaration = true
                    };

                    typeDeclaration = current switch
                    {
                        "class" => TypeDeclaration.CLASS,
                        "record" => TypeDeclaration.RECORD,
                        _ => typeDeclaration
                    };

                    result.Add(token);
                    break;
                }
            }

            if (typeDeclaration == TypeDeclaration.NONE)
                return [];

            foreach (var line in formattedInput[1..])
            {
                var token = new Token();
                var currentLine = GetCurrentLineArray(line, typeDeclaration);

                for (var j = 0; j < currentLine.Length; j++)
                {
                    var current = currentLine[j];
                    if (string.IsNullOrWhiteSpace(current))
                        break;

                    if (current.StartsWith("//"))
                    {
                        var comment = string.Join(" ", currentLine).Replace("//", "");
                        token.IsComment = true;
                        token.Comment = comment;
                        break;
                    }

                    if (_tokenType.Contains(current.ToLower()))
                    {
                        token.Type = current;
                        continue;
                    }

                    if (token.Type == null && currentLine.Length - 1 > j)
                    {
                        token.Type = current;
                        continue;
                    }

                    token.Identifier = current.Replace(",","");
                }

                if (token is not { Type: null, Identifier: null } || token.IsComment)
                    result.Add(token);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
        
        return result;
    }

    private string[] GetCurrentLineArray(string input, TypeDeclaration _t)
    {
        switch (_t)
        {
            case TypeDeclaration.CLASS:
                return input.Split([" "], StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => !ignoredKeywords.Contains(c))
                    .Select(c => c.Replace("{", "").Replace("}", ""))
                    .ToArray();
            
            case TypeDeclaration.RECORD:
                return input.Split([" "], StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => !ignoredKeywords.Contains(c))
                    .Select(c => c.Replace("{", "").Replace("}", "").Replace("(", "")
                        .Replace(")", "").Replace(":", ""))
                    .ToArray();
            default:
                return [];
        }
    }
}