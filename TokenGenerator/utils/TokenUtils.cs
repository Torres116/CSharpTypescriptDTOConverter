namespace TokenGenerator.utils;

public static class TokenUtils
{
    public static string? RemoveDictionary(this string? type)
    {
        return type?.Replace("Dictionary", "").Replace("<", "").Replace(">", "");
    }
    
    public static string? RemoveGenerics(this string? type)
    {
        return type?.Split("<").First();
    }
    
}