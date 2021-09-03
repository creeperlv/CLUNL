using CLUNL.ConsoleAppHelper;
using System;

namespace SampleConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleAppHelper.Init("Sample", "Sample");
            ConsoleAppHelper.Execute(args);
        }
    }
    [DependentFeature("Sample", "Feature0", Description = "Sample Description", Options = new string[] { "Output,O", "Version,V" },
        OptionDescriptions = new string[] { "Output location.", "Specified version." })]
    public class Feature0 : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            Output.Out(new WarnMsg { ID = "Warning.000", Fallback = "Test" });
        }
    }
    [DependentVersion("Sample")]
    public class SampleVersion : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return typeof(Program).Assembly.GetName().Version + "-Preview";
        }
    }
}
