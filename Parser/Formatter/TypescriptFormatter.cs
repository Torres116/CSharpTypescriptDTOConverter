
namespace Parser.Formatter;

public class TypescriptFormatter(FormatOptions formatOptions) 
{

    public string GetIdent()
    {
        var ident = new string(' ', formatOptions.IdentSize * formatOptions.IdentLevel);
        return ident;
    }

    public string GetTab()
    {
        var tab = new string(' ', formatOptions.TabSize);
        return tab;
    }

    public static string Format(string text)
    {
        return text;
    }
    
}