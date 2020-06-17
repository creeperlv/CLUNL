using System;
using System.Collections.Generic;

namespace CLUNL.Pipeline
{
    public interface IPipedProcessUnit
    {
        object Process(object Input);
    }
    public interface IPipelineProcessor : IPipedProcessUnit
    {
        void Init();
    }
    public class UniversalProcessor:IPipelineProcessor
    {
        List<IPipedProcessUnit> processUnits = new List<IPipedProcessUnit>();

        public void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                if (item.FullName.StartsWith("System.")) continue;
                if (item.FullName.StartsWith("Microsoft.")) continue;
                if (item.FullName.StartsWith("netstandard")) continue;
                {
                    var types=item.GetTypes();
                    foreach (var type0 in types)
                    {
                        if (type0.IsAssignableFrom(typeof(IPipelineProcessor)))
                        {
                            continue;
                        }
                        if (type0.IsAssignableFrom(typeof(IPipedProcessUnit)))
                        {
                            processUnits.Add(Activator.CreateInstance(type0) as IPipedProcessUnit);
                        }
                    }
                }
            }
        }

        public object Process(object Input)
        {

            foreach (var item in processUnits)
            {
                Input = item.Process(Input);
            }
            return Input;
        }
    }

    public class DefaultProcessor : IPipelineProcessor
    {
        List<IPipedProcessUnit> processUnits = new List<IPipedProcessUnit>();
        public void Init(params IPipedProcessUnit[] units)
        {
            if (units.Length == 0)
            {
                processUnits.Add(new DefaultProcessUnit());
            }
        }
        public void Init()
        {
            Init(new IPipedProcessUnit[0]);
        }

        public object Process(object Input)
        {

            foreach (var item in processUnits)
            {
                Input = item.Process(Input);
            }
            return Input;
        }
    }
    public class DefaultProcessUnit : IPipedProcessUnit
    {
        public object Process(object Input)
        {
            return Input;
        }
    }
}
