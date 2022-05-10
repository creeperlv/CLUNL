using CLUNL.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CLUNLTests
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void TypeResolverTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                {
                    var t = SerializableGraph.FindType("CLUNLTests.SampleType");
                    Trace.WriteLine(t);
                    Assert.IsNotNull(t);
                }
            }
        }
    }
}
