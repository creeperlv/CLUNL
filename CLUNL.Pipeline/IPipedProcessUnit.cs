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
    [Serializable]
    public class PipelineDataContinuityException : Exception
    {
        public PipelineDataContinuityException(IPipedProcessUnit unit) : base("Pipeline Data continuity is broken. Caused by unit:" + unit.GetType().FullName) { }
        public PipelineDataContinuityException(IPipedProcessUnit unit, Exception inner) : base("Pipeline Data continuity is broken. Caused by unit:" + unit.GetType().FullName, inner) { }
        protected PipelineDataContinuityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class DefaultProcessUnit : IPipedProcessUnit
    {
        public PipelineData Process(PipelineData Input)
        {
            return Input;
        }
    }
}
