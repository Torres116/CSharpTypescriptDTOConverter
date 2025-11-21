using System.Text.RegularExpressions;
using TokenGenerator;
using TokenGenerator.utils;

namespace Parser;

internal sealed partial class Lexer
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

    public List<Token> Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];


        input = RemoveSpaceAfter().Replace(input, "<"); // remove spaces after '<'
        input = RemoveSpacesAroundComma().Replace(input, ","); // remove spaces around commas
        input = RemoveSpacesBefore().Replace(input, ">"); // remove spaces before '>'

        var result = new List<Token>();
        var separators = new[] { "\n" };
        var formattedInput = input
            .Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();

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

                    if (_tokenType.Contains(current.ToLower()) || token.Type == null && currentLine.Length - 1 > j)
                    {
                        token.Type = current;
                        continue;
                    }

                    token.Identifier = current.Replace(",", "");
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

    private string[] GetCurrentLineArray(string input, TypeDeclaration type)
    {
        return type switch
        {
            TypeDeclaration.CLASS => input.Split([" "], StringSplitOptions.RemoveEmptyEntries)
                .Where(c => !ignoredKeywords.Contains(c))
                .Select(c => c.Replace("{", "").Replace("}", ""))
                .ToArray(),

            TypeDeclaration.RECORD => input.Split([" "], StringSplitOptions.RemoveEmptyEntries)
                .Where(c => !ignoredKeywords.Contains(c))
                .Select(c => c.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace(":", ""))
                .ToArray(),
            _ => []
        };
    }

    [GeneratedRegex(@"<\s*")]
    private static partial Regex RemoveSpaceAfter();

    [GeneratedRegex(@"\s*,\s*")]
    private static partial Regex RemoveSpacesAroundComma();

    [GeneratedRegex(@"\s*>")]
    private static partial Regex RemoveSpacesBefore();
}