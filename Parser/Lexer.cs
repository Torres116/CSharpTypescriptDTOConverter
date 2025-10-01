using System.Text.RegularExpressions;

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

    private readonly string[] _declarationKeywords =
    [
        "class",
        "struct",
        "interface",
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
        "timespan",
    ];


    public List<Token.Token> Tokenize(string input)
    {
        var result = new List<Token.Token>();
        var separators = new[]{"\n"};
        var formattedInput = input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();
        
        // Remove generic whitespaces
        var genericTypePattern = @"(?<=<[^>]*)\s+(?=[^<]*>)";
        for (int k = 0; k < formattedInput.Length; k++)
        {
            formattedInput[k] = Regex.Replace(formattedInput[k], genericTypePattern, "");
        }
        
        try
        {
            for (var i = 0; i < formattedInput.Length; i++)
            {
                var token = new Token.Token();
                var currentLine = formattedInput[i].Split([" "], StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => !ignoredKeywords.Contains(c))
                    .Select(c => c.Replace("{","").Replace("}",""))
                    .ToArray();
                
                for (var j = 0; j < currentLine.Length; j++)
                {
                    var current = currentLine[j];
                    
                    // TODO: Change this
                    if (_declarationKeywords.Contains(current.ToLower()))
                    {
                        j += 1;
                        token.Type = "class";
                        token.Identifier = currentLine[j];
                        token.IsTypeDeclaration = true;
                        continue;
                    }
                    
                    if (string.IsNullOrWhiteSpace(current) || current.StartsWith("//"))
                        break;
                        
                    if (_tokenType.Contains(current.ToLower()))
                    {
                        token.Type = current;
                        continue;
                    } 
                    
                    if (token.Type == null && currentLine.Length - 1 > j )
                        token.Type = current;

                    token.Identifier = current;
                }
                
                if (token is { Type: not null, Identifier: not null })
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
    

}

