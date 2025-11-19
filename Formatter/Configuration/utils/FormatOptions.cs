namespace Formatter.Configuration.utils;

public static class FormatOptions
{
    public static readonly List<TypeDeclaration> TypeDeclarations =
    [
       TypeDeclaration.Interface,
       TypeDeclaration.Class,
       TypeDeclaration.Type
    ];

    public static readonly List<NamingConvention> NamingConventions =
    [
        NamingConvention.camelCase,
        NamingConvention.PascalCase,
        NamingConvention.snake_case
    ];

    public static readonly List<int> TabSize =
    [
        2,
        4,
        8
    ];

    public static readonly List<int> IdentSize =
    [
        2,
        4,
        8
    ];

    
}