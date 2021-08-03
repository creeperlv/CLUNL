using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Indicates that a class is a version provider.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DependentVersionAttribute : Attribute
    {
        readonly string featureCollectionID;
        /// <summary>
        /// Initializes a new instance of the `DependentVersionAttribute` class.
        /// </summary>
        /// <param name="FeatureCollectionID"></param>
        public DependentVersionAttribute(string FeatureCollectionID)
        {
            this.featureCollectionID = FeatureCollectionID;

        }
        /// <summary>
        /// The ID of the collection, in case an assembly contains features that should be accessed in separated dependent application.
        /// </summary>

        public string FeatureCollectionID
        {
            get { return featureCollectionID; }
        }
        //public int NamedInt { get; set; }
    }
}