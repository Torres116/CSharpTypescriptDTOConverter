namespace Formatter.Formatter.handlers;

public static class NamingConventionHandler
{
    public static string ToCamelCase(string str)
    {
        var result = str[0].ToString().ToLower();
        for (var i = 1; i < str.Length; i++)
        {
            if (str[i] == '_')
            {
                while (i < str.Length && str[i] == '_')
                    i++;
                
                result += char.ToUpper(str[i]);
                continue;
            }
            
            result += str[i];
        }

        return result;
    }

    public static string ToPascalCase(string str)
    {
        var result = str[0].ToString().ToUpper();
        for (var i = 1; i < str.Length; i++)
        {
            if (str[i] == '_')
            {
                while (i < str.Length && str[i] == '_')
                    i++;
                
                result += char.ToUpper(str[i]);
                continue;
            }
            
            result += str[i];
        }

        return result;
    }

    public static string ToSnakeCase(string str)
    {
        var result = "";
        str = ToPascalCase(str);
        
        for (var i = 0; i < str.Length; i++)
        {
            var c = str[i];
            if (char.IsUpper(c) && i > 0)
            {
                c = char.ToLower(c);
                if (i + 1 < str.Length)
                {
                    result += "_" + c;
                    continue;
                }
                result += c;
            }
            
            result += c.ToString().ToLower();
        }
        
        return result;
    }
}