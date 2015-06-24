using System.Collections.Generic;
using System.Linq;

namespace System.Web.Optimization.RequireJs
{
    public static class RequireJs
    {
        public static IHtmlString Config(string baseUrl, params string[] bundles)
        {
            if (bundles == null)
                throw new ArgumentNullException("bundles");

            var defines = new List<String>();

            foreach (var path in bundles)
            {
                var files = BundleResolver.Current.GetBundleContents(path);
                var filenamesByPath = files.ToDictionary(f => ModuleRegistry.GetModuleName(f), f => f);

                if (BundleTable.EnableOptimizations)
                {
                    defines.Add(string.Format("'{0}': ['{1}']", Scripts.Url(path), string.Join("','", filenamesByPath.Keys)));
                }
                else
                {
                    defines.Add(String.Join(",", filenamesByPath.Select(k =>
                    {
                        var absolutPath = VirtualPathUtility.ToAbsolute(k.Value);
                        var absolutePathWithoutExtension = absolutPath.Substring(0, absolutPath.LastIndexOf('.'));
                        return string.Format("'{0}': '{1}'", k.Key, absolutePathWithoutExtension);
                    })));
                }
            }

            var absoluteBaseUrl = VirtualPathUtility.ToAbsolute(baseUrl);
            var propertyName = BundleTable.EnableOptimizations ? "bundles" : "paths";
            return new HtmlString(String.Format("require.config({{baseUrl: '{0}', {1}: {{{2}}}}});", absoluteBaseUrl, propertyName, string.Join(",", defines)));
        }
    }
}