using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUNL.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLUNLTests
{
    [TestClass]
    public class CommandLineAnalyzerTest
    {
        [TestMethod]
        public void Test0()
        {
            var cmd = "foo.exe --On -Key\\\\:0:V0;V1 -Key1:V0;V1;V2 \"-bar=item0;item1;item2\"";
            var cmdl = CommandLineTool.Analyze(cmd);
            Console.WriteLine("Argument Count:" + cmdl.RealParameter.Count);
            bool ShowDetails = false;
            if (ShowDetails)
                foreach (var item in cmdl.RealParameter)
                {
                    Console.WriteLine("Argument:" + item.EntireArgument);
                    if (item.isSwitch == true)
                    {
                        Console.WriteLine("\tSwitch:" + item.SwitchName);
                    }
                    if (item.isCollection == true)
                    {
                        Console.WriteLine("\tCollection:" + item.CollectionName);
                        foreach (var item2 in item.Collection)
                        {
                            Console.WriteLine("\tCollection Item:" + item2);
                        }
                    }
                }
            Console.WriteLine("Is ON on:" + cmdl.IsOn("ON"));
            Console.WriteLine("Is on on:" + cmdl.IsOn("on"));
            Console.WriteLine("Collection: bar>>");
            foreach (var item in cmdl.GetValueList("bar"))
            {
                Console.WriteLine(item);
            }
        }

    }
}
