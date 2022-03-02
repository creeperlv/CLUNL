using CLUNL.ConsoleAppHelper;
using CLUNL.Localization;
using System;

namespace SampleConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleAppHelper.Init("Sample", "Sample");
            {
                var codes = Language.EnumerateLanguageCodes();
                foreach (var item in codes)
                {
                    Console.WriteLine(item);
                }
            }
            ConsoleAppHelper.Colorful = true;
            ConsoleAppHelper.PreExecution = () =>
            {
                Output.OutLine("This software/library is licensed under the MIT Licenes.");
            };
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
