using System.Text;
using Formatter.Formatter.handlers;
using Formatter.interfaces;
using Formatter.Options;
using Formatter.Options.utils;

namespace Formatter.Formatter;

public class TypescriptFormatter : IFormatter
{
    private StringBuilder sb { get; } = new();

    private static string GetTypeDeclaration()
    {
        return FormatOptions.TypeDeclaration switch
        {
            TypeDeclaration.Class => "class",
            TypeDeclaration.Interface => "export interface",
            TypeDeclaration.Enum => "enum",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string GetIdent()
    {
        var ident = new string(' ', FormatOptions.IdentSize * FormatOptions.IdentLevel);
        return ident;
    }

    private static string GetTab()
    {
        var tab = new string(' ', FormatOptions.TabSize);
        return tab;
    }

    public void FormatTypeDeclaration(string identifier)
    {
        var declaration = $"{GetTypeDeclaration()} {identifier} " + "{";
        sb.Append(declaration);
        sb.AppendLine();
    }

    private void EndTypeDeclaration()
    {
        sb.Append("}");
    }

    private static string FormatNamingConvention(string identifier)
    {
        return FormatOptions.NamingConvention switch
        {
            NamingConvention.CamelCase => NamingConventionHandler.ToCamelCase(identifier),
            NamingConvention.PascalCase => NamingConventionHandler.ToPascalCase(identifier),
            NamingConvention.SnakeCase => NamingConventionHandler.ToSnakeCase(identifier),
            _ => identifier
        };
    }

    public void FormatLine(string identifier, string type)
    {
        if (FormatOptions.TypeDeclaration != TypeDeclaration.Interface)
            return;

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(type))
            return;

        identifier = FormatNamingConvention(identifier);

        //TODO: Change this
        sb.Append(GetIdent());
        sb.Append(identifier);
        sb.Append(':');
        sb.Append(GetTab());
        sb.Append(type);
        sb.Append(';');
        sb.AppendLine();
    }

    public string GetResult()
    {
        EndTypeDeclaration();
        return sb.ToString();
    }
}