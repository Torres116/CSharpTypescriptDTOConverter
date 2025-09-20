namespace Parser;

/// <summary>
/// Represents a lexer that tokenizes input text into structured tokens for further parsing.
/// </summary>
/// <remarks>
/// This class is responsible for processing input text, ignoring certain predefined keywords,
/// and creating token objects with type and identifier information. It handles the formatting of textual
/// input and identifies valid tokens based on specific criteria.
/// </remarks>
internal class Lexer
{
    /// <summary>
    /// Represents a collection of keywords that are ignored during the tokenization process
    /// in the <c>Lexer</c> class. These keywords are typically related to access modifiers
    /// or specific C# syntax elements that are not necessary for token categorization.
    /// </summary>
    readonly string[] ignoredKeywords =
    [
        "public",
        "private",
        "protected",
        "internal",
        "async"
        
    ];

    /// <summary>
    /// Represents an array of strings containing the types of tokens
    /// that the lexer recognizes during the tokenization process.
    /// </summary>
    readonly string[] tokenType =
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


    /// <summary>
    /// Tokenizes the input string into a list of tokens based on predefined rules for identifiers and types.
    /// </summary>
    /// <param name="input">The input string to be tokenized, typically containing code or text to analyze.</param>
    /// <returns>A list of tokens, where each token represents a significant element of the input, including its type and identifier.</returns>
    public List<Token.Token> Tokenize(string input)
    {
        var result = new List<Token.Token>();
        var separators = new[]{"\n"};
        var formattedInput = input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.Replace(";", "").Replace("\n", "").Replace("\t", ""))
            .ToArray();
        
        try
        {
            for (var i = 0; i < formattedInput.Length; i++)
            {
                var token = new Token.Token();
                var current = formattedInput[i].Split([" "], StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => !ignoredKeywords.Contains(c))
                    .Select(c => c.Replace("{","").Replace("}","").Replace("get","").Replace("set",""))
                    .ToArray();
                
                for (var j = 0; j < current.Length; j++)
                {
                    
                    if (string.IsNullOrWhiteSpace(current[j]) || current[j].StartsWith("//"))
                        break;
                        
                    if (tokenType.Contains(current[j].ToLower()))
                    {
                        token.Type = current[j];
                        continue;
                    } 
                    
                    if (token.Type == null && current.Length - 1 > j )
                        token.Type = current[j];
                    
                    if (current[j] == "class")
                    {
                        token.isTypeDeclaration = true;
                        continue;
                    }
                    
                    token.Identifier = current[j];
                }
                
                if (token is { Type: not null, Identifier: not null })
                    result.Add(token);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return result;
    }
    

};

