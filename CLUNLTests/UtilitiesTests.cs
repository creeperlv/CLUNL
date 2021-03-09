using CLUNL.Scripting;
using CLUNL.Scripting.SMS;
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
            CLUNL.Scripting.ScriptEnvironment environment = new CLUNL.Scripting.ScriptEnvironment();
            environment.Expose("Console", typeof(Console));
            simpleManagedScript.SetEnvironmentBase(environment);
            
            string script = @"

NEW B Bool
SET Result Int:0
LABEL L0
ADD Result Int Result 1
EXEC Console WriteLine E:Result
LGR B Int:50 E:Result#A Comment
IF B L0
";
            var obj=simpleManagedScript.Eval(script,out errors);
            Trace.WriteLine(obj + "");
            if (errors.Count != 0)
            {
                foreach (var item in errors)
                {

                    Trace.WriteLine(""+item.Message);
                }
            }
        }
    }
}
