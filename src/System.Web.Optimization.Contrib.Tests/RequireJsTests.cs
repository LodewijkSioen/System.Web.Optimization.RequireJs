using Shouldly;

namespace System.Web.Optimization.Contrib.Tests
{
    public class ModuleNameTransformTests
    {
        readonly ModuleNameTransform _transform = new ModuleNameTransform("Name");

        public void ScriptWithDefineShouldHaveModuleNameAdded()
        {
            _transform.Process("~/test", "Loads of Javascript; define(['dep1', 'dep2'], function(a,b){}); Even More javascript")
                .ShouldBe("Loads of Javascript; define('Name', ['dep1', 'dep2'], function(a,b){}); Even More javascript");
        }

        public void ScriptWithDefineThatAlreadyHasAModuleNameShouldNotBeAdded()
        {
            _transform.Process("~/jquery", @"if ( typeof define === ""function"" && define.amd ) {	define( ""jquery"", [], function() {	return jQuery;	});}")
                .ShouldBe(@"if ( typeof define === ""function"" && define.amd ) {	define( ""jquery"", [], function() {	return jQuery;	});}");
        }

        public void ProcessingFileShouldAddItToTheRegistry()
        {
            ModuleRegistry.Clear();
            _transform.Process("~/test", "Bla bla bla");

            ModuleRegistry.GetModuleName("~/test").ShouldBe("Name");
        }
    }
}