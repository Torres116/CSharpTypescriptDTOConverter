using System.Text;
using Parser.Parsers;
using Parser.Token;

namespace Parser;

public sealed class ParseService
{
    Config _config;
    
    public string ParseText(string text,Config cfg)
    {
        _config = cfg;
        return Parse(text);
    }

    string Parse(string text)
    {
        try
        {
            var lexer = new Lexer();
            var parser = ParserFactory.GetParser(_config.parser);
            var tokens = lexer.Tokenize(text);
            var parsedResult = parser.Parse(tokens);
            Console.WriteLine(parsedResult.Result);
            return parsedResult.Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "";
        }

    }

    
}