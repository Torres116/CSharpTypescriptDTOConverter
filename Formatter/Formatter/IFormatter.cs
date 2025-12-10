namespace Formatter.Formatter;

public interface IFormatter
{
    void Format(
        List<(string Identifier, string Type, bool IsComment, string? Comment, bool IsDeclaration, bool IsCustomType,
            string[]? CustomTypes)> tokens);
}