using Formatter.Formatter;
using Parser.Interfaces;
using TokenGenerator;
using TokenGenerator.interfaces;

namespace Parser.Parsers;

internal sealed class TypescriptParser : IParser
{
    private readonly TypescriptTokenGenerator _generator = new();
    private readonly ConversionResult _conversionResult = new();

    public async Task<ConversionResult> Parse(List<IToken> rawTokens, CancellationToken ct = default)
    {
        _conversionResult.Output = await Build(rawTokens, ct);
        return _conversionResult;
    }

    Task<string> Build(List<IToken> tokens, CancellationToken ct)
    {
        foreach (var token in tokens)
        {
            ct.ThrowIfCancellationRequested();
            var newToken = _generator.ConvertToken(token);
            _conversionResult.ParsedTokens.Add(newToken);
        }

        var ft = new TypescriptFormatter();
        ft.Format(_conversionResult.ParsedTokens.Select(token => (
                token.Identifier, token.Type, token.IsComment, token.Comment,
                token.IsDeclaration, token.IsCustomType, token.CustomTypes)).ToList()
        );

        return Task.FromResult(ft.GetResult());
    }
}