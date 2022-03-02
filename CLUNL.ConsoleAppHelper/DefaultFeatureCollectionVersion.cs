using System.Reflection;

namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Default version provider.
    /// </summary>
    public class DefaultFeatureCollectionVersion : IFeatureCollectionVersion
    {
        Assembly asm;
        /// <summary>
        /// Initializes a new instance of the `DefaultFeatureCollectionVersion` class.
        /// </summary>
        /// <param name="asm"></param>
        public DefaultFeatureCollectionVersion(Assembly asm)
        {
            this.asm = asm;
        }
        /// <summary>
        /// Returns the version of the assembly.
        /// </summary>
        /// <returns></returns>
        public string GetVersionString()
        {
            return asm.GetName().Version.ToString();
        }
    }
}
