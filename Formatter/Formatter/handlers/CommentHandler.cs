using System.Text;
using Formatter.Configuration;

namespace Formatter.Formatter.handlers;

internal static class CommentHandler
{
    public static void AddComment(string? comment, StringBuilder sb)
    {
        if(string.IsNullOrWhiteSpace(comment))
            return;
        
        sb.Append($"{FormatConfiguration.GetIdent()}// {comment}");
        sb.AppendLine();
    }
}