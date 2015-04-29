using Moq;
using Shouldly;

namespace System.Web.Optimization.Contrib.Tests
{
    public class AjaxMinJsMinifyTests
    {
        private readonly AjaxMinJsMinify _ajaxMinJsMinify;
        private readonly BundleContext _context;
        private readonly BundleResponse _response;
        private const string UnminifiedJs = "//this is a comment\r\nvar test = \"Hello World\";\r\nconsole.log(test);";
        private const string MinifiedJs = "var test=\"Hello World\";console.log(test);\n//# sourceMappingURL=bundle.map\n";

        public AjaxMinJsMinifyTests()
        {
            _ajaxMinJsMinify = new AjaxMinJsMinify();
            var httpContext = new Mock<HttpContextBase>();
            var bundleCollection = new BundleCollection();
            _context = new BundleContext(httpContext.Object, bundleCollection, "~/bundle");
            _response = new BundleResponse
            {
                Content = UnminifiedJs
            };
        }

        public void TestMinificationEnabled()
        {   
            _ajaxMinJsMinify.Process(_context, _response);

            _response.Content.ShouldBe(MinifiedJs);
        }

        public void TestMinificationDisabled()
        {
            _context.EnableOptimizations = false;
            _ajaxMinJsMinify.Process(_context, _response);

            _response.Content.ShouldBe(UnminifiedJs);
        }

        public void TestMinificationShouldPublishSourceMaps()
        {
            _ajaxMinJsMinify.Process(_context, _response);

            var sourceMapBundle = _context.BundleCollection.GetBundleFor("~/bundle.map");
            sourceMapBundle.Builder.BuildBundleContent(sourceMapBundle, _context, null).ShouldBe("{\r\n\"version\":3,\r\n\"file\":\"bundle\",\r\n\"lineCount\":1,\r\n\"mappings\":\"A,I,K,a,C,O,I,C,I,C\",\r\n\"sources\":[],\r\n\"names\":[\"test\",\"console\",\"log\"]\r\n}\r\n");
        }
    }
}
