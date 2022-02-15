using System;
using System.Collections.Generic;

namespace CLUNL.CacheManager
{
    /// <summary>
    /// The list of caches.
    /// </summary>
    [Serializable]
    public class CacheList
    {
        /// <summary>
        /// Domains
        /// </summary>
        public List<Domain> Domains = new List<Domain>();
    }
}
