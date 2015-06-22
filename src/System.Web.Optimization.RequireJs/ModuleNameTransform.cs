using System.Text.RegularExpressions;

namespace System.Web.Optimization.RequireJs
{
    public class RequireModule : IItemTransform
    {
        private readonly string _moduleName;
        private readonly Regex _defineRegex = new Regex(@"(define\s*\(\s*)(?=[\[f])");

        public RequireModule(string moduleName)
        {
            _moduleName = moduleName;
        }

        public string Process(string includedVirtualPath, string input)
        {
            ModuleRegistry.Define(_moduleName, includedVirtualPath);

            return _defineRegex.Replace(input, "define('"+_moduleName+"', "); //The regex takes the opening bracket as well
        }
    }
}
