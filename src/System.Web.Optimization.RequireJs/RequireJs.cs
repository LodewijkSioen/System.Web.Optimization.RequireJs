using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;

namespace System.Web.Optimization.RequireJs
{
    public static class RequireJs
    {
        private static string _absoluteBaseUrl= VirtualPathUtility.ToAbsolute("~/");

        public static void SetBaseUrl(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }
            if (!baseUrl.StartsWith("~"))
            {
                throw new ArgumentException("BaseUrl needs to start with needs to be virtual (start with '~')", "baseUrl");
            }

            _absoluteBaseUrl = VirtualPathUtility.ToAbsolute(VirtualPathUtility.AppendTrailingSlash(baseUrl));
        }

        public static Bundle IncludeModuleDirectory(this Bundle bundle, string directoryVirtualPath)
        {
            var directory = BundleTable.VirtualPathProvider.GetDirectory(directoryVirtualPath);

            foreach (var file in directory.Files)
            {
                var moduleName = ((VirtualFile)file).VirtualPath.DeriveModuleName().TrimBaseUrl(_absoluteBaseUrl);
                bundle.Include("~" + ((VirtualFile)file).VirtualPath, new RequireModule(moduleName));
            }

            return bundle;
        }

        public static IHtmlString Config(string bundlePath)
        {
            if (bundlePath == null)
                throw new ArgumentNullException("bundlePath");

            var defines = new List<String>();

            var files = BundleResolver.Current.GetBundleContents(bundlePath);
            var filenamesByPath = files.ToDictionary(f => ModuleRegistry.GetModuleName(f), f => f);

            if (BundleTable.EnableOptimizations)
            {
                var moduleNames = filenamesByPath.Keys.Select(k => k.TrimBaseUrl(_absoluteBaseUrl));
                defines.Add(string.Format("'{0}': ['{1}']", Scripts.Url(bundlePath), string.Join("','", moduleNames)));
            }
            else
            {
                defines.Add(String.Join(",", filenamesByPath.Select(k =>
                {
                    var absolutePath = Scripts.Url(k.Value).ToString();
                    var moduleName = k.Key.TrimBaseUrl(_absoluteBaseUrl);
                    var modulePath = absolutePath.RemoveJsFromFileName().TrimBaseUrl(_absoluteBaseUrl);
                    
                    return string.Format("'{0}': '{1}'", moduleName, modulePath);
                })));
            }


            var propertyName = BundleTable.EnableOptimizations ? "bundles" : "paths";
            return new HtmlString(String.Format("require.config({{baseUrl: '{0}', {1}: {{{2}}}}});", _absoluteBaseUrl, propertyName, string.Join(",", defines)));
        }
    }
}