using System.Text;
using Formatter.Configuration;

namespace Formatter.Formatter;

internal class ImportsManager
{
    private StringBuilder Sb { get; } = new();
    private List<string> Ignored { get; } = new();
    private HashSet<string> TypesAdded { get; } = new();
    public void IgnoreImport(string type) => Ignored.Add(type);
    public void IgnoreImport(string[] types) => Ignored.AddRange(types);
    
    public void AddImport(string[] types)
    {
        types = types.Where(c => !Ignored.Contains(c)).ToArray();
        foreach (var type in types.Where(c => !TypesAdded.Contains(c)))
        {
            var str = $@"import type {type} from ""./{type}"";";

            TypesAdded.Add(type);
            Sb.AppendLine(str);
        }
    }
    
    public string GetImports()
    {
        Sb.AppendLine();
        return Sb.ToString();
    }
}