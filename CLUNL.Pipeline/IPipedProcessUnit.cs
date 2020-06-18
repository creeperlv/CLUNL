using System;
using System.Collections.Generic;

namespace CLUNL.Pipeline
{
    public interface IPipedProcessUnit
    {
        PipelineData Process(PipelineData Input);
    }
    public interface IPipelineProcessor : IPipedProcessUnit
    {
        void Init();
        PipelineData Process(PipelineData Input, bool IgnoreError);
    }
    public class UniversalProcessor : IPipelineProcessor
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
                    var types = item.GetTypes();
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

        public PipelineData Process(PipelineData Input)
        {
            return Process(Input, false);
        }

        public PipelineData Process(PipelineData Input, bool IgnoreError)
        {
            if (IgnoreError)
            {

                foreach (var item in processUnits)
                {
                    try
                    {
                        var output = item.Process(Input);
                        if (Input.CheckContinuity(output))
                        {
                            Input = output;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return Input;
            }
            else
            {

                foreach (var item in processUnits)
                {
                    var output = item.Process(Input);
                    if (Input.CheckContinuity(output))
                    {
                        Input = output;
                    }
                    else
                    {
                        throw new PipelineDataContinuityException(item);
                    }
                }
                return Input;
            }
        }
    }

    [Serializable]
    public class PipelineDataContinuityException : Exception
    {
        public PipelineDataContinuityException(IPipedProcessUnit unit) : base("Pipeline Data continuity is broken. Caused by unit:" + unit.GetType().FullName) { }
        public PipelineDataContinuityException(IPipedProcessUnit unit, Exception inner) : base("Pipeline Data continuity is broken. Caused by unit:" + unit.GetType().FullName, inner) { }
        protected PipelineDataContinuityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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

        public PipelineData Process(PipelineData Input)
        {
            return Process(Input, false);
        }

        public PipelineData Process(PipelineData Input, bool IgnoreError)
        {
            if (IgnoreError)
            {

                foreach (var item in processUnits)
                {
                    try
                    {
                        var output = item.Process(Input);
                        if (Input.CheckContinuity(output))
                        {
                            Input = output;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return Input;
            }
            else
            {

                foreach (var item in processUnits)
                {
                    var output = item.Process(Input);
                    if (Input.CheckContinuity(output))
                    {
                        Input = output;
                    }
                    else
                    {
                        throw new PipelineDataContinuityException(item);
                    }
                }
                return Input;
            }
        }
    }
    public class DefaultProcessUnit : IPipedProcessUnit
    {
        public PipelineData Process(PipelineData Input)
        {
            return Input;
        }
    }
}
