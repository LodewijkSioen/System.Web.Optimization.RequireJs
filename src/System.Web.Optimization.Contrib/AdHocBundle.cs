using System.Collections.Generic;

namespace System.Web.Optimization.Contrib
{
    public class AdHocBundle : Bundle
    {
        public AdHocBundle(string virtualPath)
            : base(virtualPath)
        {
            Builder = new AdhocBundleBuilder();
        }

        public string Content { get; set; }
    }

    public class AdhocBundleBuilder : IBundleBuilder
    {
        public string BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files)
        {
            var adHocBundle = bundle as AdHocBundle;
            if (adHocBundle == null)
            {
                throw new ArgumentException("todo", "bundle");
            }
            return adHocBundle.Content;
        }
    }
}
