using System;
using System.Collections.Generic;

namespace CLUNL
{
    /// <summary>
    /// This class contains some basic library info.
    /// </summary>
    public class LibraryInfo
    {
        /// <summary>
        /// Library version.
        /// </summary>
        public static readonly Version LibVersion = new Version(0, 1, 0, 0);
        internal static Dictionary<int, int> __ = new Dictionary<int, int>();
        /// <summary>
        /// Get the library version as string.
        /// </summary>
        /// <returns></returns>
        public static string GetVersionString()
        {
            var c = BuildInfo.Channel;
            if (BuildInfo.Channel == "Undefined00")
            {
                c = "PreRelease";
            }
            return $"{LibVersion}-{c}";
        }
        /// <summary>
        /// Set a flag into given value.
        /// </summary>
        /// <param name="FeatureID"></param>
        /// <param name="Value"></param>
        public static void SetFlag(int FeatureID, int Value)
        {
            if (__.ContainsKey(FeatureID))
            {
                __[FeatureID] = Value;
            }
            else
            {
                __.Add(FeatureID, Value);
            }
        }
        /// <summary>
        /// Get the value of target flag.
        /// </summary>
        /// <param name="FeatureID"></param>
        /// <returns>-1 by default</returns>
        public static int GetFlag(int FeatureID)
        {
            return __.ContainsKey(FeatureID) == true ? __[FeatureID] : -1;
        }
    }
    /// <summary>
    /// FeatureFlags.
    /// </summary>
    public static class FeatureFlags
    {
        /// <summary>
        /// Whether ListData will auto save.
        /// </summary>
        public static int ListData_AutoSave = 0x0001;
        /// <summary>
        /// Whether HoldableObject will throw an exception when it is on hold.
        /// </summary>
        public static int ThrowExceptionWhenHold = 0x0002;
        /// <summary>
        /// Whether FileWR will create file when given file do not exist.
        /// </summary>
        public static int FileWR_AutoCreateFile = 0x0003;
        /// <summary>
        /// Whether pipeline will ignore pipeline unit error.
        /// </summary>
        public static int Pipeline_IgnoreError = 0x0004;
        /// <summary>
        /// Whether Pipeline will auto generate an ID.
        /// </summary>
        public static int Pipeline_AutoID_Random = 0x0005;
    }
    /// <summary>
    /// Version Channel
    /// </summary>
    public enum VersionChannel
    {
        /// <summary>
        /// The version is stable.
        /// </summary>
        Stable,
        /// <summary>
        /// The version is unstable.
        /// </summary>
        Testing
    }
}
