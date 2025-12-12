using System.Text.RegularExpressions;
using TokenGenerator;
using TokenGenerator.interfaces;

namespace Parser;

internal sealed partial class Lexer
{
    private static readonly HashSet<string> IgnoredKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "public",
        "private",
        "protected",
        "internal",
        "async",
        "enum"
    };

    private static readonly HashSet<string> DeclarationKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "class",
        "record"
    };

    private static readonly HashSet<string> Type = new(StringComparer.OrdinalIgnoreCase)
    {
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
    };

    public List<IToken> Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        var formattedInput = GetLines(input);
        var result = new List<IToken>();

        try
        {
            foreach (var line in formattedInput)
            {
                var token = CreateToken(line);
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

    // Format input 
    private IEnumerable<string> GetLines(string input)
    {
        input = RemoveSpaceAfter().Replace(input, "<"); // remove spaces after '<'
        input = RemoveSpacesBefore().Replace(input, ">"); // remove spaces before '>'

        var separators = new[] { "\n" };
        var formattedInput = input
            .Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();

        return formattedInput;
    }

    // Remove spaces and split line
    private string[] SplitLine(string input)
    {
        input = RemoveSpacesAroundComma().Replace(input, ",");
        return input.Split([" "], StringSplitOptions.RemoveEmptyEntries)
            .Where(c => !IgnoredKeywords.Contains(c))
            .Select(c => c.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace(":", ""))
            .ToArray();
    }

    private IToken CreateToken(string line)
    {
        var token = new Token();
        var currentLine = SplitLine(line);

        for (var j = 0; j < currentLine.Length; j++)
        {
            var current = currentLine[j];

            if (DeclarationKeywords.Contains(current) && j < currentLine.Length - 1)
            {
                var identifier = currentLine[++j];
                token = new Token
                {
                    Type = string.Empty,
                    Identifier = identifier,
                    IsDeclaration = true
                };

                return token;
            }

            if (string.IsNullOrWhiteSpace(current))
                break;

            if (current.StartsWith("//"))
            {
                var comment = string.Join(" ", currentLine).Replace("//", "");
                token.IsComment = true;
                token.Comment = comment;
                break;
            }

            if (Type.Contains(current.ToLower()) || (token.Type == null && j < currentLine.Length - 1))
            {
                token.Type = current;
                continue;
            }

            token.Identifier = current.Replace(",", "");
        }

        return token;
    }

    [GeneratedRegex(@"<\s*")]
    private static partial Regex RemoveSpaceAfter();

    [GeneratedRegex(@"\s*,\s*")]
    private static partial Regex RemoveSpacesAroundComma();

    [GeneratedRegex(@"\s*>")]
    private static partial Regex RemoveSpacesBefore();
}