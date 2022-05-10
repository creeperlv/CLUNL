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
            SerializableGraph g = new SerializableGraph();
            for (int i = 0; i < 10000; i++)
            {
                {
                    var t = g.FindType("CLUNLTests.SampleType");
                    Trace.WriteLine(t);
                    Assert.IsNotNull(t);
                }
            }
        }
    }
    public class SampleType
    {

    }
}
