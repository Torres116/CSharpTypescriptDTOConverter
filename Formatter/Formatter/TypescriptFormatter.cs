using System.Text;
using Formatter.Configuration;
using Formatter.Configuration.utils;
using Formatter.Formatter.handlers;

namespace Formatter.Formatter;

public sealed class TypescriptFormatter : IFormatter
{
    private StringBuilder Sb { get; } = new();
    private ImportsManager ImportsManager { get; } = new();
    private ConstructorManager ConstructorManager { get; } = new();
    private StringBuilder Result { get; } = new();
    private bool HasOpenType { get; set; }

    private void FormatLine(string identifier, string type)
    {
        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(type))
            return;

        FormatNamingConvention(ref identifier);
        Sb.Append(FormatIdentifier(identifier));
        Sb.AppendLine(FormatType(type));

        Console.WriteLine(FormatConfiguration.HasConstructor);
        if (FormatConfiguration.HasConstructor)
            ConstructorManager.FormatConstructorParameter(identifier);
    }

    private string GetTypeDeclaration()
    {
        return FormatConfiguration.TypeDeclaration switch
        {
            TypeDeclaration.Class => "export class",
            TypeDeclaration.Interface => "export interface",
            TypeDeclaration.Type => "export type",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void FormatTypeDeclaration(string identifier)
    {
        if (HasOpenType)
            EndTypeDeclaration();

        ImportsManager.IgnoreImport(identifier);

        var declaration = FormatConfiguration.TypeDeclaration == TypeDeclaration.Type
            ? $"{GetTypeDeclaration()} {identifier} = {{"
            : $"{GetTypeDeclaration()} {identifier} {{";

        Sb.AppendLine(declaration);
        ConstructorManager.InitializeConstructor(identifier);
        HasOpenType = true;
    }

    private void EndTypeDeclaration()
    {
        if (FormatConfiguration.IncludeConstructor && FormatConfiguration.TypeDeclaration == TypeDeclaration.Class)
            Sb.Append(ConstructorManager.GetConstructor());
        Sb.AppendLine("}");
        Sb.AppendLine();
        HasOpenType = false;
    }

    private static void FormatNamingConvention(ref string identifier)
    {
        identifier = FormatConfiguration.NamingConvention switch
        {
            NamingConvention.camelCase => NamingConventionHandler.ToCamelCase(identifier),
            NamingConvention.PascalCase => NamingConventionHandler.ToPascalCase(identifier),
            NamingConvention.snake_case => NamingConventionHandler.ToSnakeCase(identifier),
            _ => identifier
        };
    }

    private string FormatIdentifier(string identifier)
    {
        return $"{FormatConfiguration.GetIdent()}{identifier}:";
    }

    private string FormatType(string type)
    {
        return $"{FormatConfiguration.GetTab()}{type};";
    }

    public string GetResult()
    {
        Result.Append(ImportsManager.GetImports());
        EndTypeDeclaration();
        Result.Append(Sb);
        return Result.ToString().Trim();
    }

    public void Format(
        List<(string Identifier, string Type, bool IsComment, string? Comment, bool IsDeclaration, bool IsCustomType,
            string[]? CustomTypes)> tokens)
    {
        ImportsManager.IgnoreImport(tokens.Where(c => c.IsDeclaration).Select(c => c.Identifier).ToArray());

        foreach (var token in tokens)
        {
            if (token.IsComment && FormatConfiguration.IncludeComments)
            {
                CommentHandler.AddComment(token.Comment, Sb);
                continue;
            }

            if (token.IsDeclaration)
            {
                ImportsManager.IgnoreImport(token.Identifier);
                FormatTypeDeclaration(token.Identifier);
                continue;
            }

            if (token.IsCustomType && FormatConfiguration.IncludeImports)
                ImportsManager.AddImport(token.CustomTypes ?? []);

            FormatLine(token.Identifier, token.Type);
        }
    }

    public void Reset()
    {
        Sb.Clear();
    }
}