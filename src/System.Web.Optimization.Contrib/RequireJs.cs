using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.Optimization.Contrib
{
    public static class RequireJs
    {
        private static readonly ConcurrentDictionary<string, string> Defines = new ConcurrentDictionary<string, string>();

        public static IHtmlString Render(params string[] paths)
        {
            if (paths == null)
                throw new ArgumentNullException("paths");

            var defines = new List<String>();

            foreach (var path in paths)
            {
                var files = BundleResolver.Current.GetBundleContents(path);
                var filenamesByPath = files.ToDictionary(f => Path.GetFileNameWithoutExtension(f), f => f);

                if (BundleTable.EnableOptimizations)
                {
                    defines.Add(string.Format("'{0}': ['{1}']", Scripts.Url(path), string.Join("','", filenamesByPath.Keys)));
                }
                else
                {
                    defines.Add(String.Format(String.Join(",", filenamesByPath.Select(k => string.Format("'{0}': '{1}'", k.Key, k.Value)))));
                }
            }

            var propertyName = BundleTable.EnableOptimizations ? "bundles" : "paths";
            return new HtmlString(String.Format("{0}: {{{1}}}", propertyName, string.Join(",", defines)));
        }

        public static Bundle Include(this Bundle bundle, string define, string virtualPath)
        {
            Defines.AddOrUpdate(define, virtualPath, (k, v) => virtualPath);
            return bundle.Include(virtualPath);
        }
    }
}