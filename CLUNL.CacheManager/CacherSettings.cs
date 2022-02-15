using System;

namespace CLUNL.CacheManager
{
    /// <summary>
    /// Configuration of cache manager settings.
    /// </summary>
    public struct CacherSettings
    {
        /// <summary>
        /// How long will a cache expire after an update.
        /// </summary>
        public TimeSpan ExpireTimeSpan;
        /// <summary>
        /// How many times of usage will a cache expire after an update.
        /// </summary>
        public int ExpireUsingCount;
    }
}
