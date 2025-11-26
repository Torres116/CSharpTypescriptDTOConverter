using Parser.Parsers;

namespace Parser;

public sealed class ParseService
{
    public static Config _config;
    
    public async Task<string> ParseText(string text,Config cfg)
    {
        _config = cfg;
        return await Parse(text);
    }

    private async Task<string> Parse(string text)
    {
        try
        {
            var lexer = new Lexer();
            var parser = ParserFactory.GetParser(_config.parser);
            var rawTokens = lexer.Tokenize(text);
            var conversionResult = await parser.Parse(rawTokens);
            return conversionResult.Output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "";
        }

    }

}