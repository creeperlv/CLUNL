using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.ConsoleAppHelper
{
    public class DefaultFeatureCollectionVersion : IFeatureCollectionVersion
    {
        Assembly asm;
        public DefaultFeatureCollectionVersion(Assembly asm)
        {
            this.asm= asm;
        }
        public string GetVersionString()
        {
            return asm.GetName().Version.ToString();
        }
    }
}
