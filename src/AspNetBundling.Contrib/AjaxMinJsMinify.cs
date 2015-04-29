using Microsoft.Ajax.Utilities;
using System.Web.Optimization;

namespace AspNetBundling.Contrib
{
    public class AjaxMinJsMinify : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "application/javascript";

            if (context.EnableOptimizations)
            {
                var minifier = new Minifier();
                response.Content = minifier.MinifyJavaScript(response.Content);
            }
        }
    }
}
