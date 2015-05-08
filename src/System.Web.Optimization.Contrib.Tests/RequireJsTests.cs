using Shouldly;

namespace System.Web.Optimization.Contrib.Tests
{
    public class ModuleNameTransformTests
    {
        readonly ModuleNameTransform _transform = new ModuleNameTransform("Name");

        private const string JQueryDefine = @"if ( typeof define === ""function"" && define.amd ) {	define( ""jquery"", [], function() {	return jQuery;	});}";
        private const string KnockoutDefine = @"if (typeof define === 'function' && define['amd']) { define(['exports', 'require'], factory); }";
        private const string KnockoutDefinePostTransform = @"if (typeof define === 'function' && define['amd']) { define('Name', ['exports', 'require'], factory); }";
        private const string DomReadyDefine = @"define(function () { return domReady; });";
        private const string DomReadyDefinePostTransform = @"define('Name', function () { return domReady; });";

        public void ScriptWithDefineShouldHaveModuleNameAdded()
        {
            _transform.Process("~/knockout", KnockoutDefine)
                .ShouldBe(KnockoutDefinePostTransform);

            _transform.Process("~/domReady", DomReadyDefine)
                .ShouldBe(DomReadyDefinePostTransform);
        }

        public void ScriptWithDefineThatAlreadyHasAModuleNameShouldNotBeAdded()
        {
            _transform.Process("~/jquery", JQueryDefine)
                .ShouldBe(JQueryDefine);
        }

        public void ProcessingFileShouldAddItToTheRegistry()
        {
            ModuleRegistry.Clear();
            _transform.Process("~/test", "Bla bla bla");

            ModuleRegistry.GetModuleName("~/test").ShouldBe("Name");
        }
    }
}