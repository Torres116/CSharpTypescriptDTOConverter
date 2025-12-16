using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Parser;

public class SyntaxHelper
{
    public static List<string> GetSyntaxErrors(string input,CancellationToken ct = default)
    {
        var tree = CSharpSyntaxTree.ParseText(input);
        var errors = tree.GetDiagnostics(ct);
        return errors.Where(e => e.Severity == DiagnosticSeverity.Error)
            .Select(e => e.ToString()).ToList();
    }
}