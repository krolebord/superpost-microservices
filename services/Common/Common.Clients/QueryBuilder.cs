using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Common.Clients;

public class QueryBuilder
{
    private readonly List<string> parameters = new();

    public void AddParameter(string key, string? value)
    {
        parameters.Add($"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}");
    }
    
    public QueryBuilder AddParameter(string key, object value)
    {
        if (value is IEnumerable values)
        {
            foreach (var parameter in values)
            {
                AddParameter(key, parameter!.ToString());
            }
        }
        else
        {
            AddParameter(key, value.ToString());
        }
        return this;
    }

    public override string ToString()
    {
        if (parameters.Count == 0)
        {
            return string.Empty;
        }
        
        var query = string.Join("&", parameters);
        return $"?{query}";
    }
}
