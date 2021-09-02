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
    [DependentFeature("Sample","Feature0")]
    public class Feature0 : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            Output.Out(new WarnMsg { ID = "Warning.000", Fallback = "Test" });
        }
    }
}
