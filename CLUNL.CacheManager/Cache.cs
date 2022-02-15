using CLUNL.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CLUNL.CacheManager
{
    /// <summary>
    /// A cache entry.
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// The url of the request.
        /// </summary>
        public string RequestUrl;
        /// <summary>
        /// The path were the item is stored.
        /// </summary>
        public string StorePath;
        /// <summary>
        /// The time of when the cache is updated.
        /// </summary>
        public DateTime RequestDate;
        /// <summary>
        /// Requeset count.
        /// </summary>
        public int RequestCount;
        /// <summary>
        /// Obtain the file.
        /// </summary>
        /// <returns></returns>
        public FileInfo ObtainFile()
        {
            RequestCount++;
            return new FileInfo(StorePath);
        }
        /// <summary>
        /// Update the file.
        /// </summary>
        /// <returns></returns>
        public async Task<FileInfo> UpdateFile()
        {
            Logger.WriteLine("Updating cache...");
            var f = new FileInfo(StorePath);
            if (!f.Directory.Exists) f.Directory.Create();
            try
            {
                using (var t = await Cacher.client.GetAsync(RequestUrl))
                {
                    if (File.Exists(StorePath))
                        File.Delete(StorePath);
                    using (var __f = File.Create(StorePath))
                    {
                        await t.Content.CopyToAsync(__f);

                    }
                }


            }
            catch (Exception)
            {
                return null;
            }
            RequestDate = DateTime.Now;
            RequestCount = 0;
            Logger.WriteLine($"Cached -> {StorePath}");
            return f;
        }
        /// <summary>
        /// If the cache is valid.
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            if (File.Exists(StorePath))
            {
                return !isExpired();
            }
            else
            {
                Logger.WriteLine("Invalid Cache: Cache fild not found.");
                return false;
            }
        }
        /// <summary>
        /// If the cache is expired.
        /// </summary>
        /// <returns></returns>
        public bool isExpired()
        {

            var NOW = DateTime.Now;
            var __delta = NOW - this.RequestDate;
            if (__delta.Duration() > TimeSpan.FromDays(1))
            {
                Logger.WriteLine(MessageLevel.Warn, "Cache Expired for time.");
                return true;
            }
            if (RequestCount > 5)
            {
                Logger.WriteLine(MessageLevel.Warn, "Cache Expired for using count.");
                return true;
            }
            return false;
        }
    }
}
