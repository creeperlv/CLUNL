using System;
using System.Collections.Generic;

namespace CLUNL.Pipeline
{
    public class NameSpecifiedProcessor : IPipelineProcessor
    {
        List<IPipedProcessUnit> processUnits = new List<IPipedProcessUnit>();

        public void Init()
        {

            throw new Exception("Please specify process units.");
        }


        public void Init(ProcessUnitManifest manifest)
        {
            processUnits = manifest.GetUnitInstances();
        }
        public void Init(List<String> manifest)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                if (item.FullName.StartsWith("System.")) continue;
                if (item.FullName.StartsWith("Microsoft.")) continue;
                if (item.FullName.StartsWith("netstandard")) continue;
                {
                    foreach (var processUnit in manifest)
                    {
                        try
                        {
                            processUnits.Add((IPipedProcessUnit)item.CreateInstance(processUnit));
                        }
                        catch (Exception)
                        {
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

        public void Init(ProcessUnitManifest manifest)
        {
            processUnits = manifest.GetUnitInstances();
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

        public void Init(ProcessUnitManifest manifest)
        {
            processUnits = manifest.GetUnitInstances();
        }
        public PipelineData Process(PipelineData Input)
        {
            bool willIgnore = false;
            try
            {
                if (LibraryInfo.GetFlag(FeatureFlags.Pipeline_IgnoreError) == 1)
                {
                    willIgnore = true;
                }
            }
            catch (Exception)
            {
                //Ignore
            }
            return Process(Input, willIgnore);
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

}