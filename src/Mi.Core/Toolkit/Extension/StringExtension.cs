using System.Text.RegularExpressions;

namespace Mi.Core.Toolkit.Extension
{
    public static class StringExtension
    {
        public static bool RegexValidate(this string str, string pattern)
        {
            return new Regex(pattern).IsMatch(str);
        }
    }
}