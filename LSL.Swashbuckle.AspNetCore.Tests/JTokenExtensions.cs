using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace LSL.Swashbuckle.AspNetCore.Tests;

public static class JTokenExtensions
{
    public static JToken RemoveFields(this JToken token, string[] fields)
    {
        if (token is not JContainer container) return token;

        List<JToken> removeList = [];

        foreach (JToken el in container.Children())
        {
            if (el is JProperty p && fields.Contains(p.Name))
            {
                removeList.Add(el);
            }
            el.RemoveFields(fields);
        }

        foreach (JToken el in removeList)
        {
            el.Remove();
        }

        return token;
    }
}