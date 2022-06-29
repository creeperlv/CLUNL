namespace CLUNL.Pipeline.Generic
{
    public interface IPipelineProcessor<T,U,V> : IPipedProcessUnit<PipelineData<T, U, V>>
    {
        void Init();
        void Init(ProcessUnitManifest manifest);
        PipelineData<T, U, V> Process(PipelineData<T, U, V> Input, bool IgnoreError);
    }
}
