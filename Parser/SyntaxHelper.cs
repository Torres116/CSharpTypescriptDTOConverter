using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Parser;

public class SyntaxHelper
{
    public static List<string> GetSyntaxErrors(string input)
    {
        var tree = CSharpSyntaxTree.ParseText(input);
        var errors = tree.GetDiagnostics();
        return errors.Where(e => e.Severity == DiagnosticSeverity.Error)
            .Select(e => e.ToString()).ToList();
    }
}