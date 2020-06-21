using System;
using System.Collections;
using System.Linq;
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
        void Init(ProcessUnitManifest manifest);
        PipelineData Process(PipelineData Input, bool IgnoreError);
    }
    public class ProcessUnitManifest : IList<Type>
    {
        List<Type> Data = new List<Type>();
        public Type this[int index] { get => Data[index]; set => Data[index] = value; }

        public int Count => Data.Count;

        public bool IsReadOnly => true;

        public void Add(Type item)
        {
            Data.Add(item);
        }
        public List<IPipedProcessUnit> GetUnitInstances(){
            List<IPipedProcessUnit> units=new List<IPipedProcessUnit>();
            foreach (var item in Data)
            {
                units.Add((IPipedProcessUnit)Activator.CreateInstance(item));
            }
            if(units.Count==0){
                units.Add(new DefaultProcessUnit());
            }
            return units;
        }
        public void Clear()
        {
            Data.Clear();
        }

        public bool Contains(Type item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(Type[] array, int ArrayIndex)
        {
            Data.CopyTo(array, ArrayIndex);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public int IndexOf(Type item)
        {
            return Data.IndexOf(item);
        }

        public void Insert(int index, Type item)
        {
            Data.Insert(index, item);
        }

        public bool Remove(Type item)
        {
            return Data.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Data.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
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
            processUnits=manifest.GetUnitInstances();
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
        
        public void Init(ProcessUnitManifest manifest)
        {
            processUnits=manifest.GetUnitInstances();
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
    public class DefaultProcessUnit : IPipedProcessUnit
    {
        public PipelineData Process(PipelineData Input)
        {
            return Input;
        }
    }
}
