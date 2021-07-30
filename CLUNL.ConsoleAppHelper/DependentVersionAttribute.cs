using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.ConsoleAppHelper
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DependentVersionAttribute : Attribute
    {
        readonly string featureCollectionID;
        public DependentVersionAttribute(string FeatureCollectionID)
        {
            this.featureCollectionID = FeatureCollectionID;

        }

        public string FeatureCollectionID
        {
            get { return featureCollectionID; }
        }
        public int NamedInt { get; set; }
    }
}