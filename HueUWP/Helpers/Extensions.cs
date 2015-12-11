using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueUWP.Helpers
{
        public static class Extensions
        {
            public static string ToDelimitedString<T>(this IEnumerable<T> source, Func<T, string> func)
            {
                return ToDelimitedString(source, ", ", func);
            }

            public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter, Func<T, string> func)
            {
                return String.Join(delimiter, source.Select(func).ToArray());
            }

            public static bool IsNullOrEmpty(this JToken token)
            {
                return (token == null) ||
                       (token.Type == JTokenType.Array && !token.HasValues) ||
                       (token.Type == JTokenType.Object && !token.HasValues) ||
                       (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                       (token.Type == JTokenType.Null);
            }
    }
}
