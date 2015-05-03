namespace System.Web.Optimization.Contrib
{
    public class AjaxMinScriptBundle : Bundle
    {
        public AjaxMinScriptBundle(string virtualPath)
            : this(virtualPath, null)
        {
            
        }

        public AjaxMinScriptBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, null)
        {
            Builder = new AjaxMinScriptBundleBuilder();
        }
    }
}
