using Formatter.Configuration;

namespace TokenGenerator.Handlers;

public sealed class CleanupHandler
{
    public static void Cleanup(Token token)
    {
        token.Type = token.Type.Replace("?", "");
        
        if (!FormatConfiguration.IncludeOptionals)
            token.Identifier = token.Identifier!.Replace("?","");
    }
}