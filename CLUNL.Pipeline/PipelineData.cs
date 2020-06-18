using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Pipeline
{
    public class PipelineData
    {
        public object PrimaryData;
        public object SecondaryData;
        public object Options { get; }
        public PipelineData(object PrimaryData,object SecondaryData,object Options)
        {
            this.PrimaryData = PrimaryData;
            this.SecondaryData = SecondaryData;
            if (Options == null) this.Options = new object();
            else
            this.Options = Options;
        }
        public bool CheckContinuity(PipelineData Target)
        {
            return CheckContinuity(this, Target);
        }
        public static bool CheckContinuity(PipelineData Original,PipelineData New)
        {
            return Original.Options.Equals(New.Options);
        }
    }
}
