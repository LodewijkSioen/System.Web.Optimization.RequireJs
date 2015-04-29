using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace System.Web.Optimization.Contrib.TestSite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundle",new AjaxMinJsMinify())
                .Include());
        }
    }
}