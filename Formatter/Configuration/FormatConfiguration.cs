using Formatter.Configuration.utils;

namespace Formatter.Configuration;

public static class FormatConfiguration
{
    public static int TabSize { get; set; } = 2;
    public static int IdentSize { get; set; } = 2;
    public static TypeDeclaration TypeDeclaration { get; set; } = TypeDeclaration.Interface;
    public static NamingConvention NamingConvention { get; set; } = NamingConvention.camelCase;
    public static bool IncludeConstructor { get; set; }
    public static bool DatesAsStrings { get; set; } = false;
    public static bool IncludeComments { get; set; } = false;
    public static bool IncludeNullables { get; set; } = false;
    public static bool IncludeOptionals { get; set; } = true;
    public static bool IncludeImports { get; set; } = true;
    public static bool HasConstructor => IncludeConstructor && TypeDeclaration == TypeDeclaration.Class;

    public static void TurnEverythingOff()
    {
        IncludeConstructor = false;
        DatesAsStrings = false;
        IncludeComments = false;
        IncludeNullables = false;
        IncludeOptionals = false;
        IncludeImports = false;
    }

    public static string GetIdent()
    {
        var ident = new string(' ', IdentSize);
        return ident;
    }

    public static string GetWhiteSpace(int count = 1)
    {
        var ident = new string(' ', count);
        return ident;
    }

    public static string GetTab(int? tabSize = null)
    {
        var tab = new string(' ', tabSize ?? TabSize);
        return tab;
    }
}