using TokenGenerator;

namespace Parser.Interfaces;

internal interface IParser
{
    Task<string> Parse(List<Token> tokens);
}