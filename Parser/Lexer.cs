using System.Text.RegularExpressions;
using Formatter.Options;
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
        "timespan"
    ];


    public List<Token> Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];
        
        var result = new List<Token>();
        var separators = new[]{"\n"};
        var formattedInput = input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();
        
        // Remove generic whitespaces
        const string genericTypePattern = @"(?<=<[^>]*)\s+(?=[^<]*>)";
        for (var k = 0; k < formattedInput.Length; k++)
            formattedInput[k] = Regex.Replace(formattedInput[k], genericTypePattern, "");
        
        // Remove nullable from types
        if(!FormatOptions.IncludeNullables)
           formattedInput = formattedInput.Select(c => c.Replace("?","")).ToArray();
        
        try
        {
            for (var i = 0; i < formattedInput.Length; i++)
            {
                var token = new Token();
                var currentLine = formattedInput[i].Split([" "], StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => !ignoredKeywords.Contains(c))
                    .Select(c => c.Replace("{","").Replace("}",""))
                    .ToArray();
                
                for (var j = 0; j < currentLine.Length; j++)
                {
                    var current = currentLine[j];
                    
                    if (_declarationKeywords.Contains(current.ToLower()))
                    {
                        var next = currentLine[j + 1];
                        token.Type = current ;
                        token.Identifier = next;
                        break;
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
                
                if (token is not {Type: null,Identifier: null})
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

