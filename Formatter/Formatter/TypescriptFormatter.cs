using System.Text;
using Formatter.Configuration;
using Formatter.Configuration.utils;
using Formatter.Formatter.handlers;

namespace Formatter.Formatter;

public sealed class TypescriptFormatter : IFormatter
{
    private StringBuilder Sb { get; } = new();
    private StringBuilder ConstructorSb { get; } = new();
    private StringBuilder ImportsSb { get; } = new();
    private StringBuilder Result { get; } = new();
    private List<string>? CustomTypes;
    private List<string> Ignored { get; } = new();
    private bool HasOpenType { get; set; }

    private void FormatLine(string identifier, string type)
    {
        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(type))
            return;

        identifier = FormatNamingConvention(identifier);

        FormatConstructorParameter(identifier);
        Sb.Append(FormatIdentifier(identifier));
        Sb.Append(FormatType(type));
        Sb.AppendLine();
    }

    private string GetTypeDeclaration()
    {
        return FormatConfiguration.TypeDeclaration switch
        {
            TypeDeclaration.Class => "class",
            TypeDeclaration.Interface => "interface",
            TypeDeclaration.Type => "type",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void FormatTypeDeclaration(string identifier)
    {
        if (HasOpenType)
            EndTypeDeclaration();

        Ignored.Add(identifier);
        AddExport();

        var declaration =
            FormatConfiguration.TypeDeclaration == TypeDeclaration.Type
                ? $"{GetTypeDeclaration()} {identifier} = {{"
                : $"{GetTypeDeclaration()} {identifier} {{";

        Sb.Append(declaration);
        Sb.AppendLine();
        InitializeConstructor(identifier);
        HasOpenType = true;
    }

    private void AddExport()
    {
        Sb.Append("export ");
    }

    private static string GetIdent()
    {
        var ident = new string(' ', FormatConfiguration.IdentSize * FormatConfiguration.IdentLevel);
        return ident;
    }

    private static string GetWhiteSpace(int count = 1)
    {
        var ident = new string(' ', count);
        return ident;
    }

    private static string GetTab(int? tabSize = null)
    {
        var tab = new string(' ', tabSize ?? FormatConfiguration.TabSize);
        return tab;
    }

    private void EndTypeDeclaration()
    {
        AddConstructor();
        Sb.AppendLine("}");
        Sb.AppendLine();
        HasOpenType = false;
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

    private string FormatIdentifier(string identifier)
    {
        return $"{GetIdent()}{identifier}:";
    }

    private string FormatType(string type)
    {
        return $"{GetTab()}{type};";
    }

    private void FormatComment(string comment)
    {
        Sb.Append(GetIdent());
        Sb.Append("//");
        Sb.Append(comment);
        Sb.AppendLine();
    }

    private void InitializeConstructor(string identifier)
    {
        if (!FormatConfiguration.GenerateConstructor || FormatConfiguration.TypeDeclaration != TypeDeclaration.Class)
            return;

        var declaration = $"constructor(init: {identifier}) {{ ";

        ConstructorSb.AppendLine();
        ConstructorSb.Append(GetIdent());
        ConstructorSb.Append(declaration);
        ConstructorSb.AppendLine();
    }

    private void FormatConstructorParameter(string identifier)
    {
        if (!FormatConfiguration.GenerateConstructor || FormatConfiguration.TypeDeclaration != TypeDeclaration.Class)
            return;

        identifier = identifier.Replace("?", "");

        var declaration = $"this.{identifier} =  init.{identifier}";
        ConstructorSb.Append(GetIdent());
        ConstructorSb.Append(GetWhiteSpace());
        ConstructorSb.Append(declaration);
        ConstructorSb.AppendLine(";");
    }

    private void AddConstructor()
    {
        if (!FormatConfiguration.GenerateConstructor ||
            FormatConfiguration.TypeDeclaration != TypeDeclaration.Class) return;

        Sb.Append(ConstructorSb);
        Sb.AppendLine(GetIdent() + "}");
        ConstructorSb.Clear();
    }

    private void AddImport(string[] types)
    {
        if (!FormatConfiguration.IncludeImports)
            return;

        CustomTypes ??= new();


        foreach (var type in types.Where(c => !CustomTypes.Contains(c)))
        {
            var str = $@"import type {type} from ""./{type}"";";

            CustomTypes.Add(type);
            ImportsSb.AppendLine(str);
        }
    }

    private void AddImports()
    {
        if (!FormatConfiguration.IncludeImports)
            return;

        ImportsSb.AppendLine();
        Result.Append(ImportsSb);
    }

    private void BuildMain()
    {
        EndTypeDeclaration();
        Result.Append(Sb);
    }

    public string GetResult()
    {
        AddImports();
        BuildMain();
        return Result.ToString();
    }

    public void Format(
        List<(string? Identifier, string? Type, bool IsComment, string? Comment, bool IsDeclaration, bool IsCustomType,
            string[]? CustomTypes)> tokens)
    {
        Ignored.AddRange(tokens.Where(c => c.IsDeclaration).Select(c => c.Identifier!));

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

            if (token.IsCustomType)
                AddImport(token.CustomTypes ?? []);

            FormatLine(token.Identifier!, token.Type!);
        }
    }

    public void Reset()
    {
        Sb.Clear();
        ImportsSb.Clear();
        ConstructorSb.Clear();
        CustomTypes = null;
        Ignored?.Clear();
    }
}