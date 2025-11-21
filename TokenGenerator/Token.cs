namespace TokenGenerator;

public class Token
{
    public string? Identifier;
    public string? Type;
    public bool IsDeclaration = false;
    public bool IsComment = false;
    public string? Comment;
    public bool IsCustomType = false;
}
