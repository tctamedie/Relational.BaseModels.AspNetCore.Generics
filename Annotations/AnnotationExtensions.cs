using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    internal static class AnnotationExtensions
    {
        public static string FirstLetterToLower(this string text)
        {
            //Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            var result = char.ToLower(text[0]) + text.Substring(1);
            return result;
        }
        public static string CamelSplit(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                input,
                $"(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                " $1",
                System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}
