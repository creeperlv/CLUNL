using CLUNL.Diagnostics;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.CacheManager
{
    /// <summary>
    /// The main access point of CacheManager.
    /// </summary>
    public class Cacher
    {
        internal static HttpClient client = new HttpClient();
        /// <summary>
        /// Settings of cache manager.
        /// </summary>
        public static CacherSettings Settings = new CacherSettings { ExpireTimeSpan = TimeSpan.FromDays(1), ExpireUsingCount = 10 };
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            FloatParseHandling = FloatParseHandling.Double,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml
        };
        static CacheList CurrentList;
        static string StorageBase;
        static string Index;
        /// <summary>
        /// Product Name, such as "CLUNL", then, the cache will be stored in "/tmp/CLUNL/".
        /// </summary>
        public static string ProductName="Temp";
        /// <summary>
        /// Initialize the cache manager.
        /// </summary>
        public static void Init()
        {
            StorageBase = Path.Combine(Path.GetTempPath(), ProductName);
            Index = Path.Combine(StorageBase, "index.installation");
            if (!Directory.Exists(StorageBase))
            {
                Directory.CreateDirectory(StorageBase);
                CurrentList = new CacheList();
                File.WriteAllText(Index, JsonConvert.SerializeObject(CurrentList));
            }
            else
            {
                if (File.Exists(Index))
                {
                    CurrentList = JsonConvert.DeserializeObject<CacheList>(File.ReadAllText(Index), settings);
                }
                else
                {
                    CurrentList = new CacheList();
                    File.WriteAllText(Index, JsonConvert.SerializeObject(CurrentList));
                }
            }
            AutoMaintain();
        }
        /// <summary>
        /// The interval between of maintenances.
        /// </summary>
        public static int MaintainTimeInterval = 5000;
        static void AutoMaintain()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(MaintainTimeInterval);
                    Maintain();
                }
            });
        }
        /// <summary>
        /// Force to require a maintenance.
        /// </summary>
        public static void ForceMaintain()
        {
            Maintain();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Maintain()
        {
            lock (CurrentList)
            {
                File.Delete(Index);
                File.WriteAllText(Index, JsonConvert.SerializeObject(CurrentList, settings));
                foreach (var d in CurrentList.Domains)
                {
                    foreach (var c in d.Caches)
                    {
                        if (c.isExpired())
                        {
                            if (File.Exists(c.StorePath))
                            {
                                File.Delete(c.StorePath);
                                c.RequestCount = 0;
                                Logger.WriteLine("Maintenance",$"Cache \"{c.RequestUrl}\" is now forced to be invalid due to it is expired.");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Obtain a cache.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Cache FindCache(string URL)
        {

            Uri uri = new Uri(URL);
            var __host = uri.GetComponents(UriComponents.Host, UriFormat.UriEscaped).ToLower();
            var __file = uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            var __port = uri.GetComponents(UriComponents.StrongPort, UriFormat.UriEscaped);
            var __query = uri.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
            lock (CurrentList)
            {
                foreach (var item in CurrentList.Domains)
                {
                    if (item.Host == __host && item.Port == __port)
                    {
                        foreach (var cache in item.Caches)
                        {
                            if (cache.RequestUrl == URL)
                            {
                                return cache;
                            }
                        }
                        return null;
                    }
                }
            }

            return null;
        }
        static async Task<FileInfo> CreateCache(string URL)
        {
            Logger.WriteLine($"Creating cache for {URL}.");

            Uri uri = new Uri(URL);
            var __host = uri.GetComponents(UriComponents.Host, UriFormat.UriEscaped).ToLower();
            var __file = uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            var __port = uri.GetComponents(UriComponents.StrongPort, UriFormat.UriEscaped);
            var __query = uri.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
            Domain D = null;
            lock (CurrentList)
            {

                foreach (var item in CurrentList.Domains)
                {
                    if (item.Host == __host && item.Port == __port)
                    {
                        Logger.WriteLine($"Found domain: {item.Host} : {item.Port}");
                        D = item;
                        break;
                    }
                }
            }
            if (D != null)
            {
                Cache cache = new Cache();
                cache.RequestCount = 0;
                cache.RequestDate = DateTime.Now;
                cache.RequestUrl = URL;
                cache.StorePath = Path.Combine(StorageBase, __host, __port, __file);
                var f = await cache.UpdateFile();
                lock (D)
                {
                    D.Caches.Add(cache);
                }
                return f;
            }
            else
            {
                Logger.WriteLine(MessageLevel.Warn, $"Domain not found.");
                var d = new Domain { Host = __host, Port = __port };
                lock (CurrentList)
                {
                    CurrentList.Domains.Add(d);
                }
                Logger.WriteLine($"Created domain:{d.Host}:{d.Port}");

                Cache cache = new Cache();
                cache.RequestCount = 0;
                cache.RequestDate = DateTime.Now;
                cache.RequestUrl = URL;
                cache.StorePath = Path.Combine(StorageBase, __host, __port, __file);
                var f = await cache.UpdateFile();
                d.Caches.Add(cache);
                return f;
            }
        }
        /// <summary>
        /// Request a file.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static async Task<FileInfo> Request(string URL)
        {
            Logger.WriteLine($"Request:{URL}");
            var cache = FindCache(URL);
            if (cache == null)
            {
                Logger.WriteLine(MessageLevel.Warn, $"Cache not found.");
                return await CreateCache(URL);
            }
            else
            {
                if (cache.isValid())
                {
                    return cache.ObtainFile();
                }
                else
                {
                    return await cache.UpdateFile();
                }
            }
        }
        /// <summary>
        /// Force to request a file. (Force update a cache.)
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static async Task<FileInfo> ForceRequest(string URL)
        {
            Logger.WriteLine($"Request:{URL}");
            var cache = FindCache(URL);
            if (cache == null)
            {
                Logger.WriteLine(MessageLevel.Warn, $"Cache not found.");
                return await CreateCache(URL);
            }
            else
            {
                return await cache.UpdateFile();
            }
        }
    }
}
