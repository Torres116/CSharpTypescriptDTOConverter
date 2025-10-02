using Parser.Interfaces;

namespace Parser.Parsers;

public static class ParserFactory
{
    public static IParser GetParser(string parser)
    {
        return parser switch
        {
            "Typescript" => new TypescriptParser(),
            "CSharp" => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(parser), parser,"parser doesn't exist")
        };
    }
}