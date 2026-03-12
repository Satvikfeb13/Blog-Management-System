using System.Text.RegularExpressions;

namespace BlogApp.Helper
{
    public static class RemoveHtmlTagHelper
    {
        public static string removehtmltag(string text)
        {
            return Regex.Replace(text,"<.*?>| &.*;>",string.Empty);
        }
    }
}
