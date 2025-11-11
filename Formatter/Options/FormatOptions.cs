using Formatter.Options.utils;

namespace Formatter.Options;

public static class FormatOptions
{
    public static int TabSize { get; set; } = 2;
    public static int IdentSize { get; set; } = 2;
    public static int IdentLevel { get; set; } = 1;
    public static TypeDeclaration TypeDeclaration { get; set; } = TypeDeclaration.Interface;
    public static NamingConvention NamingConvention { get; set; } = NamingConvention.CamelCase;
    public static bool GenerateConstructor { get; set; } = false;
    public static bool AddTypeAnnotations { get; set; } = true;
    public static bool DatesAsStrings { get; set; } = false;
    public static bool IncludeComments { get; set; } = false;
    public static bool IncludeNullables { get; set; } = true;
    public static bool SetAllOptionals { get; set; } = false;

}