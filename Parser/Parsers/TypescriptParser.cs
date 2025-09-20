using System.Text;
using Parser.Context;
using Parser.Formatter;
using Parser.Interfaces;
using Parser.Token;
using Parser.TokenGenerator;

namespace Parser.Parsers;

public class TypescriptParser : IParser
{

    public Task<string> Parse(List<Token.Token> tokens)
    {
        if (tokens.Count is 0 || !tokens.Any(c => c.Type == "class"))
            return Task.FromResult(string.Empty);
        
        var generator = new TypescriptTokenGenerator();
        List<TypescriptToken> tsTokens =
            tokens.Select(token => generator.InterpretToken(token)).ToList();

        var result = Build(tsTokens);
        return Task.FromResult(result);
    }

    string Build(List<TypescriptToken> tokens)
    {
        var sb = new StringBuilder();
        var fo = new FormatOptions();
        var ft = new TypescriptFormatter(fo);
        
        foreach (var token in tokens)
        {
            if (token.Type == "class")
            {
                var typeStructure = $"interface {token.Identifier} " + "{" ;
                sb.Append(typeStructure);
                sb.AppendLine();
                continue;
            }
            
            var ident = ft.GetIdent();
            var tab = ft.GetTab();

            sb.Append(ident);
            sb.Append(token.Identifier);
            sb.Append(":");
            sb.Append(tab);
            sb.Append(token.Type);
            sb.Append(";");
            sb.AppendLine();
        }

        sb.Append("}");
        
        return sb.ToString();
    }
       
}