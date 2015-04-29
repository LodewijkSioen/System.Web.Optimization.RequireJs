using System.IO;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace System.Web.Optimization.Contrib
{
    public class AjaxMinJsMinify : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "application/javascript";
            var mapBundleVirtualPath = string.Concat(context.BundleVirtualPath, ".map");

            if (context.EnableOptimizations)
            {
                var mapBuilder = new StringBuilder();
                using (var writer = new StringWriter(mapBuilder))
                using(var sourceMap = new V3SourceMap(writer))
                {
                    var minifier = new Minifier();
                    var settings = new CodeSettings
                    {
                        EvalTreatment = EvalTreatment.MakeImmediateSafe,
                        PreserveImportantComments = false,
                        SymbolsMap = sourceMap,
                        TermSemicolons = true
                    };
                    
                    sourceMap.StartPackage(context.BundleVirtualPath, mapBundleVirtualPath);
                    response.Content = minifier.MinifyJavaScript(response.Content, settings);
                }
                
                context.BundleCollection.Add(new AdHocBundle(mapBundleVirtualPath){Content = mapBuilder.ToString()});
            }
        }
    }
}
