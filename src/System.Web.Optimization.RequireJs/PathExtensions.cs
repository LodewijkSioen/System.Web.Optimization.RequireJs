namespace System.Web.Optimization.RequireJs
{
    public static class PathExtensions
    {
        public static string DeriveModuleName(this string path)
        {
            return (path.EndsWith(".html") ? string.Concat("text!", path) : path).RemoveJsFromFileName();
        }

        public static string RemoveJsFromFileName(this string path)
        {
            return path.EndsWith(".js") ? path.Substring(0, path.LastIndexOf('.')) : path;
        }

        public static string TrimBaseUrl(this string path, string baseUrl)
        {
            if (path.StartsWith(baseUrl))
            {
                return path.Substring(baseUrl.Length);
            }
            if(path.StartsWith(string.Concat("text!", baseUrl)))
            {
                return string.Concat("text!", path.Substring(5 + baseUrl.Length));
            }

            return path;
        }
    }
}