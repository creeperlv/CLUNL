using System;

namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Indicates that a class is a console Application feature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DependentFeatureAttribute : Attribute
    {
        readonly string nameString;
        readonly string featureCollectionID;
        /// <summary>
        /// Initializes a new instance of the `DependentFeatureAttribute` class.
        /// </summary>
        /// <param name="featureCollectionID">The ID of the collection, in case an assembly contains features that should be accessed in separated dependent application.</param>
        /// <param name="nameString"></param>
        public DependentFeatureAttribute(string featureCollectionID, string nameString)
        {
            this.nameString = nameString;
            this.featureCollectionID = featureCollectionID;
        }

        /// <summary>
        /// The ID of the collection, in case an assembly contains features that should be accessed in separated dependent application.
        /// </summary>
        public string FeatureCollectionID
        {
            get => featureCollectionID;
        }
        /// <summary>
        /// The name of the feature.
        /// </summary>
        public string Name
        {
            get
            {
                return nameString;
            }
        }
        /// <summary>
        /// Description of the feature.
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Available options.
        /// </summary>
        public string[] Options
        {
            get; set;
        }
        /// <summary>
        /// Option descriptions.
        /// </summary>
        public string[] OptionDescriptions
        {
            get; set;
        }
    }
}
