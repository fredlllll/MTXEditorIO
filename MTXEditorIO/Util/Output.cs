using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MTXEditorIO.Util
{
    public static class Output
    {
        public static string Indent(string str, int spaces = 2)
        {
            string rep = "\n";
            string start = "";
            for (int i = 0; i < spaces; ++i)
            {
                rep += " ";
                start += " ";
            }
            return start + str.Replace("\n", rep);
        }

        public static string ToString<T>(IEnumerable<T> array, int indentCount = 2, int limit = 15, Func<T, string>? stringifier = null)
        {
            int count = array.Count();
            if (count == 0)
            {
                return "[]";
            }
            string retval = "[\n";
            if (stringifier == null)
            {
                stringifier = (x) => x.ToString();
            }

            retval += string.Join("\n,\n", array.Take(limit).Select((x) => Indent(stringifier(x), indentCount)));
            if (count > limit)
            {
                retval += $"\n,\n... {count - limit} truncated elements";
            }
            retval += "\n]";

            return retval;
        }

        public static string ToString<T>(this T val, string seperator = " ")
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            StringBuilder sb = new StringBuilder();
            foreach (var field in fields)
            {
                var fieldVal = field.GetValue(val);
                sb.Append($"{field.Name}: {fieldVal}{seperator}");
            }
            return sb.ToString();
        }
    }
}
