﻿using System.Collections.Concurrent;

namespace System.Web.Optimization.Contrib
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
            return DefinesByVirtualPath[virtualPath];
        }

        public static void Clear()
        {
            DefinesByVirtualPath.Clear();
        }
    }
}