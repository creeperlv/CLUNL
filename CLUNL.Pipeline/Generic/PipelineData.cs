using System;

namespace CLUNL.Pipeline.Generic
{
    [Serializable]
    public class PipelineData<T,U,V>
    {
        static Random RandomID = new Random();
        public T PrimaryData;
        public U SecondaryData;
        public V Options { get; }
        public int DataID { get; } = -1;
        public PipelineData(T PrimaryData, U SecondaryData, V Options, int DataID = -1)
        {
            this.PrimaryData = PrimaryData;
            this.SecondaryData = SecondaryData;
            if (Options == null) this.Options = default;
            else
                this.Options = Options;
            if (DataID == -1)
            {
                if (LibraryInfo.GetFlag(FeatureFlags.Pipeline_AutoID_Random) == 1)
                {
                    this.DataID = RandomID.Next();
                }
                else
                    this.DataID = this.Options.GetHashCode();
            }
        }
        public bool CheckContinuity(PipelineData<T,U,V> Target)
        {
            return CheckContinuity(this, Target);
        }
        public static bool CheckContinuity(PipelineData<T, U, V> Original, PipelineData<T, U, V> New)
        {
            return Original.DataID.Equals(New.DataID);
        }
    }
}
