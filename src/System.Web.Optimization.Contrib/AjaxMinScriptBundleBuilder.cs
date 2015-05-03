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
            var mapBundleVirtualPath = string.Concat(context.BundleVirtualPath, ".map");
            var minified = string.Empty;

            if (context.EnableOptimizations)
            {
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

                AddAdHocBundle(mapBuilder.ToString(), mapBundleVirtualPath, context);
            }
            return minified;
        }

        private string Concat(IEnumerable<BundleFile> files, BundleContext context)
        {
            var contentConcated = new StringBuilder();

            foreach (var file in files)
            {
                // Get the contents of the bundle,
                // noting it may have transforms applied that could mess with any source mapping we want to do
                var contents = file.ApplyTransforms();

                // If there were transforms that were applied
                if (file.Transforms.Count > 0)
                {
                    // Write the transformed contents to another Bundle
                    var fileVirtualPath = file.IncludedVirtualPath;
                    var virtualPathTransformed = "~/" + Path.ChangeExtension(fileVirtualPath, string.Concat(".transformed", Path.GetExtension(fileVirtualPath)));
                    AddAdHocBundle(contents, virtualPathTransformed, context);
                }

                // Source header line then source code
                contentConcated.AppendLine("///#source 1 1 " + file.VirtualFile.VirtualPath);
                contentConcated.AppendLine(contents);
            }

            return contentConcated.ToString();
        }

        private void AddAdHocBundle(string content, string virtualPath, BundleContext context)
        {
            var mapBundle = context.BundleCollection.GetBundleFor(virtualPath);
            if (mapBundle == null)
            {
                mapBundle = new AdHocBundle(virtualPath);
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