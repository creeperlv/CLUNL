using CLUNL.Scripting;
using CLUNL.Scripting.SMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
NL LIST_INT Int
NEW TEMP_INT Int
NEW TEMP_INT2 Int
SET TEMP_INT Int:0
SET Result Int:0
LABEL L0
ADD Result Int Result 1
ADDW LIST_INT E:Result
ADD TEMP_INT2 Int Result -1
LW LIST_INT E:TEMP_INT2 TEMP_INT
EXEC Console WriteLine E:TEMP_INT
LGR B Int:5 E:Result#A Comment
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
