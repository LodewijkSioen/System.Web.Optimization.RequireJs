using Moq;
using Shouldly;
using System.Web;
using System.Web.Optimization;

namespace System.Web.Optimization.Contrib.Tests
{
    public class AjaxMinJsMinifyTests
    {
        private AjaxMinJsMinify _ajaxMinJsMinify;
        private BundleContext _context;
        private BundleCollection _bundleCollection;
        private Mock<HttpContextBase> _httpContext;
        private BundleResponse _response;
        private readonly string _unminifiesJs = "//this is a comment\r\nvar test = \"Hello World\";\r\nconsole.log(test);";
        private readonly string _minifiedJs = "var test=\"Hello World\";console.log(test)";

        public AjaxMinJsMinifyTests()
        {
            _ajaxMinJsMinify = new AjaxMinJsMinify();
            _httpContext = new Mock<HttpContextBase>();
            _bundleCollection = new BundleCollection();
            _context = new BundleContext(_httpContext.Object, _bundleCollection, "~/bundle");
            _response = new BundleResponse
            {
                Content = _unminifiesJs
            };
        }

        public void TestMinificationEnabled()
        {   
            _ajaxMinJsMinify.Process(_context, _response);

            _response.Content.ShouldBe(_minifiedJs);
        }

        public void TestMinificationDisabled()
        {
            _context.EnableOptimizations = false;
            _ajaxMinJsMinify.Process(_context, _response);

            _response.Content.ShouldBe(_unminifiesJs);
        }
    }
}
