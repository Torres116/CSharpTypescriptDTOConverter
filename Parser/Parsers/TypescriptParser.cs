using System.Text;
using Formatter.Formatter;
using Parser.Interfaces;
using TokenGenerator;

namespace Parser.Parsers;

public sealed class TypescriptParser : IParser
{

    public Task<string> Parse(List<Token> tokens)
    {
        // if (tokens.Count is 0 || !tokens.Any(c => c.IsTypeDeclaration))
        //     return Task.FromResult(string.Empty);
        
        var generator = new TypescriptTokenGenerator();
        List<TypescriptToken> tsTokens =
            tokens.Select(token => generator.InterpretToken(token)).ToList();

        var result = Build(tsTokens);
        return Task.FromResult(result);
    }

    string Build(List<TypescriptToken> tokens)
    {
        var ft = new TypescriptFormatter();
        
        //TODO: Change this
        foreach (var token in tokens)
        {
            if (token.Type == "class")
            {
                ft.FormatTypeDeclaration(token.Identifier!);
                continue;
            }
            
            ft.FormatLine(token.Identifier!, token.Type!);
        }

        return ft.GetResult();
    }
       
}