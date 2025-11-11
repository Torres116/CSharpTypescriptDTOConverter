using Parser.Parsers;

namespace Parser;

public sealed class ParseService
{
    public static Config _config;
    
    public string ParseText(string text,Config cfg)
    {
        _config = cfg;
        return Parse(text);
    }

    private string Parse(string text)
    {
        try
        {
            var lexer = new Lexer();
            var parser = ParserFactory.GetParser(_config.parser);
            var tokens = lexer.Tokenize(text);
            var parsedResult = parser.Parse(tokens);
            return parsedResult.Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "";
        }

    }

}