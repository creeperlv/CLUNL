using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Pipeline
{
    [Serializable]
    public class PipelineData
    {
        static Random RandomID = new Random();
        public object PrimaryData;
        public object SecondaryData;
        public object Options { get; }
        public int DataID { get; } = -1;
        public PipelineData(object PrimaryData, object SecondaryData, object Options, int DataID = -1)
        {
            this.PrimaryData = PrimaryData;
            this.SecondaryData = SecondaryData;
            if (Options == null) this.Options = new object();
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
        public bool CheckContinuity(PipelineData Target)
        {
            return CheckContinuity(this, Target);
        }
        public static bool CheckContinuity(PipelineData Original, PipelineData New)
        {
            return Original.DataID.Equals(New.DataID);
        }
    }
}
