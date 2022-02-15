using System;
using System.Collections.Generic;

namespace CLUNL.CacheManager
{
    /// <summary>
    /// Definition of network domain.
    /// </summary>
    [Serializable]
    public class Domain
    {
        /// <summary>
        /// Hostname.
        /// </summary>
        public string Host;
        /// <summary>
        /// Port of domain
        /// </summary>
        public string Port;
        /// <summary>
        /// Caches belong to the domain
        /// </summary>
        public List<Cache> Caches = new List<Cache>();
    }
}
