using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Trace.WriteLine("Argument Count:" + cmdl.RealParameter.Count);
            bool ShowDetails = false;
            if (ShowDetails)
                foreach (var item in cmdl.RealParameter)
                {
                    Trace.WriteLine("Argument:" + item.EntireArgument);
                    if (item.isSwitch == true)
                    {
                        Trace.WriteLine("\tSwitch:" + item.SwitchName);
                    }
                    if (item.isCollection == true)
                    {
                        Trace.WriteLine("\tCollection:" + item.CollectionName);
                        foreach (var item2 in item.Collection)
                        {
                            Trace.WriteLine("\tCollection Item:" + item2);
                        }
                    }
                }
            Trace.WriteLine("Is ON on:" + cmdl.IsOn("ON"));
            Trace.WriteLine("Is on on:" + cmdl.IsOn("on"));
            Trace.WriteLine("Collection: bar>>");
            foreach (var item in cmdl.GetValueList("bar"))
            {
                Trace.WriteLine(item);
            }
        }

        [TestMethod]
        public void Test1()
        {
            var cmd = "foo.exe --On -Key\\\\:0:V0;V1 -Key1:V0;V1;V2 \"-bar=item0;item1;item2\"";
            var cmdl = CommandLineTool.Analyze(cmd);
            Trace.WriteLine("Argument Count:" + cmdl.RealParameter.Count);
            bool ShowDetails = true;
            if (ShowDetails)
                foreach (var item in cmdl.RealParameter)
                {
                    Trace.WriteLine("Argument:" + item.EntireArgument);
                    if (item.isSwitch == true)
                    {
                        Trace.WriteLine("\tSwitch:" + item.SwitchName);
                    }
                    if (item.isCollection == true)
                    {
                        Trace.WriteLine("\tCollection:" + item.CollectionName);
                        Trace.WriteLine("\tUnsegmented:" + item.UnsegmentedCollectionString);
                        foreach (var item2 in item.Collection)
                        {
                            Trace.WriteLine("\tCollection Item:" + item2);
                        }
                    }
                }
            Trace.WriteLine("Is ON on:" + cmdl.IsOn("ON"));
            Trace.WriteLine("Is on on:" + cmdl.IsOn("on"));
            Trace.WriteLine("Collection: bar>>");
            foreach (var item in cmdl.GetValueList("bar"))
            {
                Trace.WriteLine(item);
            }
        }

    }
}
