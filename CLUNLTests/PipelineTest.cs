using CLUNL.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNLTests
{
    [TestClass]
    public class PipelineTest
    {
        [TestMethod]
        public void PipeLineTest0()
        {
            DefaultProcessor defaultProcessor = new DefaultProcessor();
            defaultProcessor.Init();
            var assemblies= AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                if (item.FullName.StartsWith("System.")) continue;
                if (item.FullName.StartsWith("Microsoft.")) continue;
                if (item.FullName.StartsWith("netstandard")) continue;
                Console.WriteLine(defaultProcessor.Process(new PipelineData(item.FullName,null,null)));
            }
        }
    }
}
