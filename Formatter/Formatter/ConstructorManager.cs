using System.Text;
using Formatter.Configuration;

namespace Formatter.Formatter;

internal class ConstructorManager
{
    private StringBuilder Sb { get; } = new();

    public void InitializeConstructor(string identifier)
    {
        Sb.Clear();
        var declaration = $"\n{FormatConfiguration.GetIdent()}constructor(init: {identifier}) {{ ";
        Sb.AppendLine(declaration);
    }

    public string GetConstructor()
    {
        Sb.AppendLine(FormatConfiguration.GetIdent() + "}");
        return Sb.ToString();
    }

    public void FormatConstructorParameter(string identifier)
    {
        identifier = identifier.Replace("?", "");
        var declaration = $"this.{identifier} =  init.{identifier};";

        Sb.Append(FormatConfiguration.GetIdent());
        Sb.Append(FormatConfiguration.GetWhiteSpace());
        Sb.AppendLine(declaration);
    }
}