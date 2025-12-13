using Exceptions;
using Parser.Parsers;

namespace Parser;

public sealed class ParseService
{
    private Config _config;

    public async Task<ConversionResult> Parse(string text, Config cfg,CancellationToken ct = default)
    {
        _config = cfg;
        return await Convert(text,ct);
    }

    private async Task<ConversionResult> Convert(string text,CancellationToken ct)
    {
        var syntaxErrors = SyntaxHelper.GetSyntaxErrors(text);
        if (syntaxErrors.Count > 0)
        {
            var errors = syntaxErrors.Aggregate("", (current, error) => current + error + "\n");
            throw new SyntaxErrorException(errors);
        }

        var lexer = new Lexer();
        var parser = ParserFactory.GetParser(_config.parser);
        ConversionResult conversionResult;
        
        try
        {
            var rawTokens = lexer.Tokenize(text);
            conversionResult = await parser.Parse(rawTokens,ct);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ParseServiceException("Parsing Error.", e);
        }

        return conversionResult;
    }
}