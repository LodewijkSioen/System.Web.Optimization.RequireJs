using System.Collections.Concurrent;

namespace System.Web.Optimization.RequireJs
{
    public static class ModuleRegistry
    {
        private static readonly ConcurrentDictionary<string, string> DefinesByVirtualPath = new ConcurrentDictionary<string, string>();

        public static void Define(string moduleName, string virtualPath)
        {
            DefinesByVirtualPath.AddOrUpdate(virtualPath, s => moduleName, (s1, s2) => moduleName);
        }

        public static string GetModuleName(string virtualPath)
        {
            if (DefinesByVirtualPath.ContainsKey(virtualPath))
            {
                return DefinesByVirtualPath[virtualPath];    
            }

            return VirtualPathUtility.ToAbsolute(virtualPath).DeriveModuleName();
        }

        public static void Clear()
        {
            DefinesByVirtualPath.Clear();
        }
    }
}