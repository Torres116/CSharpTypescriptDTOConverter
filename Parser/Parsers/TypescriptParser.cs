using Formatter.Formatter;
using Parser.Interfaces;
using TokenGenerator;

namespace Parser.Parsers;

public sealed class TypescriptParser : IParser
{

    public Task<string> Parse(List<Token> tokens)
    {
        var generator = new TypescriptTokenGenerator();
        var tsTokens = tokens.Select(token => generator.InterpretToken(token));
        var result = Build(tsTokens.Where(token => token != null).ToList()!);
        return Task.FromResult(result);
    }

    string Build(List<TypescriptToken> tokens)
    {
        var ft = new TypescriptFormatter();
        
        //TODO: Change this
        foreach (var token in tokens)
        {
            if (token.IsComment)
            {
                ft.FormatComment(token.Comment!);
                continue;
            }
            
            
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