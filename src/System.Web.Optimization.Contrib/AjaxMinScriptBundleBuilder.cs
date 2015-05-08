using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace System.Web.Optimization.Contrib
{
    public class AjaxMinScriptBundleBuilder : IBundleBuilder
    {
        public string BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files)
        {
            var concatedScripts = Concat(files, context);
            var minifiedScripts = Minify(concatedScripts, context);

            return minifiedScripts;
        }

        private string Minify(string concatedScripts, BundleContext context)
        {
            //Don't use .map because that url will not always be routed trough asp.net
            var mapBundleVirtualPath = string.Concat(context.BundleVirtualPath, "-map");
            string minified;

            var mapBuilder = new StringBuilder();
            using (var writer = new StringWriter(mapBuilder))
            using (var sourceMap = new V3SourceMap(writer))
            {
                var minifier = new Minifier();
                var settings = new CodeSettings
                {
                    EvalTreatment = EvalTreatment.MakeImmediateSafe,
                    PreserveImportantComments = false,
                    SymbolsMap = sourceMap,
                    TermSemicolons = true
                };

                sourceMap.StartPackage(VirtualPathUtility.ToAbsolute(context.BundleVirtualPath), VirtualPathUtility.ToAbsolute(mapBundleVirtualPath));
                minified = minifier.MinifyJavaScript(concatedScripts, settings);
            }

            AddAdHocBundle(mapBundleVirtualPath, mapBuilder.ToString(), context);

            return minified;
        }

        private string Concat(IEnumerable<BundleFile> files, BundleContext context)
        {
            var contentConcated = new StringBuilder();

            foreach (var file in files)
            {
                var contents = file.ApplyTransforms();
                var virtualPath = file.IncludedVirtualPath;

                if (file.Transforms.Count > 0)
                {
                    virtualPath = string.Concat(virtualPath, "/transformed");
                    AddAdHocBundle(virtualPath, contents, context);
                }

                if (!Diagnostics.Debugger.IsAttached)//this hangs in ajaxmin if the debugger is attached :sadpanda:
                {
                    contentConcated.AppendLine(";///#SOURCE 1 1 " + VirtualPathUtility.ToAbsolute(virtualPath));
                }
                contentConcated.AppendLine(contents);
            }

            return contentConcated.ToString();
        }

        private void AddAdHocBundle(string virtualPath, string content, BundleContext context)
        {
            var mapBundle = context.BundleCollection.GetBundleFor(virtualPath);
            if (mapBundle == null)
            {
                mapBundle = new AdHocBundle(virtualPath);
                context.BundleCollection.Add(mapBundle);
            }
            var correctlyCastMapBundle = mapBundle as AdHocBundle;
            if (correctlyCastMapBundle == null)
            {
                throw new InvalidOperationException(string.Format("There is a bundle on the VirtualPath '{0}' of the type '{1}' when it was expected to be of the type 'SourceMapBundle'. That Virtual Path is reserved for the SourceMaps.", virtualPath, mapBundle.GetType()));
            }
            correctlyCastMapBundle.Content = content;
        }
    }
}