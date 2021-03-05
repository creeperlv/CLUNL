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
    }
}
