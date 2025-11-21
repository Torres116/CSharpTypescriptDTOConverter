using Parser.Interfaces;

namespace Parser.Parsers;

internal static class ParserFactory
{
    public static IParser GetParser(string parser)
    {
        return parser.ToLower() switch
        {
            "typescript" => new TypescriptParser(),
            "csharp" => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(parser), parser, "parser doesn't exist")
        };
    }
}