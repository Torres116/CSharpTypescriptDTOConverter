using Formatter.Formatter;
using Parser.Interfaces;
using TokenGenerator;

namespace Parser.Parsers;

internal sealed class TypescriptParser : IParser
{
    public Task<string> Parse(List<Token> tokens)
    {
        var generator = new TypescriptTokenGenerator();
        var tsTokens = tokens.Select(token => generator.InterpretToken(token)).Where(t => t != null).ToList();
        var result = Build(tsTokens.Where(token => token != null).ToList()!);
        return Task.FromResult(result);
    }

    string Build(List<TypescriptToken> tokens)
    {
        var ft = new TypescriptFormatter();

        ft.Format(tokens
            .Select(token => (token.Identifier, token.Type, token.IsComment, token.Comment, token.IsDeclaration,
                token.IsCustomType,token.CustomTypes))
            .ToList());

        return ft.GetResult();
    }
}