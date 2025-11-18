using System.Text;
using Formatter.Configuration;
using Formatter.Configuration.utils;
using Formatter.Formatter.handlers;

namespace Formatter.Formatter;

public class TypescriptFormatter : IFormatter
{
    private StringBuilder sb { get; } = new();
    private StringBuilder constructorSB { get; } = new();

    private static string GetTypeDeclaration()
    {
        return FormatConfiguration.TypeDeclaration switch
        {
            TypeDeclaration.Class => "class",
            TypeDeclaration.Interface => "interface",
            TypeDeclaration.Enum => "enum",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void AddExport()
    {
        sb.Append("export ");
    }

    private static string GetIdent()
    {
        var ident = new string(' ', FormatConfiguration.IdentSize * FormatConfiguration.IdentLevel);
        return ident;
    }
    
    private static string GetConstructorWhiteSpace()
    {
        var ident = new string(' ', 2);
        return ident;
    }

    private static string GetTab(int? tabSize = null)
    {
        var tab = new string(' ', tabSize ?? FormatConfiguration.TabSize);
        return tab;
    }

    public void FormatTypeDeclaration(string identifier)
    {
        AddExport();
        var declaration = $"{GetTypeDeclaration()} {identifier} " + "{";
        sb.Append(declaration);
        sb.AppendLine();
        InitializeConstructor(identifier);
    }

    private void EndTypeDeclaration()
    {
        if (FormatConfiguration.GenerateConstructor && FormatConfiguration.TypeDeclaration == TypeDeclaration.Class)
        {
            sb.Append(constructorSB);
            sb.Append(GetIdent());
            sb.AppendLine("}");
        }

        sb.Append("}");
    }

    private static string FormatNamingConvention(string identifier)
    {
        return FormatConfiguration.NamingConvention switch
        {
            NamingConvention.camelCase => NamingConventionHandler.ToCamelCase(identifier),
            NamingConvention.PascalCase => NamingConventionHandler.ToPascalCase(identifier),
            NamingConvention.snake_case => NamingConventionHandler.ToSnakeCase(identifier),
            _ => identifier
        };
    }

    public void FormatLine(string identifier, string type)
    {
        if (FormatConfiguration.TypeDeclaration == TypeDeclaration.Enum)
            return;

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(type))
            return;

        identifier = FormatNamingConvention(identifier);

        FormatConstructorParameter(identifier);

        //TODO: Change this
        sb.Append(GetIdent());
        sb.Append(identifier);
        sb.Append(':');
        sb.Append(GetTab());
        sb.Append(type);
        sb.Append(';');
        sb.AppendLine();
    }

    public void FormatComment(string comment)
    {
        sb.Append(GetIdent());
        sb.Append("//");
        sb.Append(comment);
        sb.AppendLine();
    }

    private void InitializeConstructor(string identifier)
    {
        if (!FormatConfiguration.GenerateConstructor || FormatConfiguration.TypeDeclaration != TypeDeclaration.Class)
            return;

        var declaration = $"constructor(init: {identifier}) {{ ";

        constructorSB.AppendLine();
        constructorSB.Append(GetIdent());
        constructorSB.Append(declaration);
        constructorSB.AppendLine();
    }

    private void FormatConstructorParameter(string identifier)
    {
        if (!FormatConfiguration.GenerateConstructor || FormatConfiguration.TypeDeclaration != TypeDeclaration.Class)
            return;

        identifier = identifier.Replace("?", "");
        
        var declaration = $"this.{identifier} =  init.{identifier}";
        constructorSB.Append(GetIdent());
        constructorSB.Append(GetConstructorWhiteSpace());
        constructorSB.Append(declaration);
        constructorSB.AppendLine(";");
    }

    public string GetResult()
    {
        EndTypeDeclaration();
        return sb.ToString();
    }

    public void Format(List<(string? Identifier, string? Type, bool IsComment,string? Comment, bool IsDeclaration)> tokens)
    {
        foreach (var token in tokens)
        {
            if (token.IsComment)
            {
                FormatComment(token.Comment ?? "");
                continue;
            }
            
            if (token.IsDeclaration)
            {
                FormatTypeDeclaration(token.Identifier!);
                continue;
            }
            
            FormatLine(token.Identifier!, token.Type!);
        }
    }
    
}