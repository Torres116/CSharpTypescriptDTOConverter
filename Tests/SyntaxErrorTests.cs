using Exceptions;
using Parser;

namespace Tests;

public sealed class SyntaxErrorTests
{
    private readonly ParseService _parseService = new();

    [Theory]
    [InlineData("public class Test { p#blic ., int Test { get; set; } }")]
    [InlineData("public class Test { publi  int Test { get; set; } }")]
    [InlineData("public class Test { public  int s Test { get; set; } }")]
    [InlineData("public class Test { public  int Test  get; set;  }")]
    [InlineData("public class Test { public  int Te,st { get; set; } }")]
    [InlineData("public class Test { public  int Test { ge set; }} ")]
    [InlineData("public class Test { public  int Test { get; set; } ")]
    private async Task ClassShouldThrowSyntaxErrorException(string input)
    {
        Func<Task> act = async Task () => await _parseService.Parse(input, new Config());

        await Assert.ThrowsAsync<SyntaxErrorException>(act);
    }

    [Theory]
    [InlineData("public class Test { public Dictionary<int,string { get; set; } }")]
    [InlineData("public class Test { public Dictionary<int> { get; set; } }")]
    [InlineData("public class Test { public Dictionary<int<> { get; set; } }")]
    [InlineData("public class Test { public Dictionary<int.> { get; set; } }")]
    private async Task DictionaryShouldThrowSyntaxErrorException(string input)
    {
        Func<Task> act = async Task () => await _parseService.Parse(input, new Config());

        var ex = await Assert.ThrowsAsync<SyntaxErrorException>(act);
    }
}