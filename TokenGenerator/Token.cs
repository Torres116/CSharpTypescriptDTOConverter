namespace TokenGenerator;

public class Token
{
    public string? Identifier;
    public string? Type;
    public bool IsDeclaration = false;
    public bool IsComment;
    public string? Comment;
}
