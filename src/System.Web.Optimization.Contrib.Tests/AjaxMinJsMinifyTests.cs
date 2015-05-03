using Moq;
using Shouldly;

namespace System.Web.Optimization.Contrib.Tests
{
    public class AjaxMinJsMinifyTests
    {
        private readonly AjaxMinScriptBundleBuilder _ajaxMinJsMinify;
        private readonly BundleContext _context;
        private readonly BundleFile[] _files;
        private readonly AjaxMinScriptBundle _bundle;
        private const string UnminifiedJs = "//this is a comment\r\nvar test = \"Hello World\";\r\nconsole.log(test);";
        private const string MinifiedJs = "var test=\"Hello World\";console.log(test);\n//# sourceMappingURL=bundle.map\n";

        private AjaxMinJsMinifyTests()
        {
            _ajaxMinJsMinify = new AjaxMinScriptBundleBuilder();
            var httpContext = new Mock<HttpContextBase>();
            var bundleCollection = new BundleCollection();
            _context = new BundleContext(httpContext.Object, bundleCollection, "~/bundle");
            _files = new[] {new BundleFile("~/file", null) };
            _bundle = new AjaxMinScriptBundle("~/bundle");
        }

        private void TestMinificationEnabled()
        {   
            var response = _ajaxMinJsMinify.BuildBundleContent(_bundle, _context, _files);

            response.ShouldBe(MinifiedJs);
        }

        private void TestMinificationDisabled()
        {
            _context.EnableOptimizations = false;
            var response = _ajaxMinJsMinify.BuildBundleContent(_bundle, _context, _files);

            response.ShouldBe(UnminifiedJs);
        }

        private void TestMinificationShouldPublishSourceMaps()
        {
            _ajaxMinJsMinify.BuildBundleContent(_bundle, _context, _files);

            var sourceMapBundle = _context.BundleCollection.GetBundleFor("~/bundle.map");
            sourceMapBundle.Builder.BuildBundleContent(sourceMapBundle, _context, null).ShouldBe("{\r\n\"version\":3,\r\n\"file\":\"bundle\",\r\n\"lineCount\":1,\r\n\"mappings\":\"A,I,K,a,C,O,I,C,I,C\",\r\n\"sources\":[],\r\n\"names\":[\"test\",\"console\",\"log\"]\r\n}\r\n");
        }
    }
}
