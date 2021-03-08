using CLUNL.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUNLTests
{
    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void ParameterResolutionTest()
        {
            var l=Utilities.ResolveParameters("This is a S:\"Test\\tString, with some\\r\\ndecorations\\\\.\" right#Comments");
            foreach (var item in l)
            {
                Trace.WriteLine($"[{item}]");
            }
        }
        [TestMethod]
        public void ScriptingTest()
        {
            SimpleManagedScript simpleManagedScript = new SimpleManagedScript();
            List<ScriptError> errors;
            CLUNL.Scripting.Environment environment = new CLUNL.Scripting.Environment();
            environment.Expose("Console", typeof(Console));
            simpleManagedScript.SetEnvironmentBase(environment);

            string script = @"
ADD Result Int 100 200
ADD Result Int Result 50
MULT Result Double Result 21.232
ADDI Result Int Result -10000
EXEC Console WriteLine String:""Hello, World!""
";
            var obj=simpleManagedScript.Eval(script,out errors);
            Trace.WriteLine(obj + "");
        }
    }
}
