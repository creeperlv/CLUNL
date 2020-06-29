using System;
using System.Collections.Generic;

namespace CLUNL
{
    public class LibraryInfo
    {
        public static readonly Version LibVersion = new Version(0, 1, 0, 0);
        internal static Dictionary<int, int> __ = new Dictionary<int, int>();
        public static void SetFlag(int FeatureID,int Value)
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
        public static int GetFlag(int FeatureID)
        {
            return __.ContainsKey(FeatureID) == true ? __[FeatureID] : -1;
        }
    }
    public static class FeatureFlags
    {
        public static int ListData_AutoSave = 0x0001;
        public static int ThrowExceptionWhenHold = 0x0002;
        public static int FileWR_AutoCreateFile = 0x0003;
        public static int Pipeline_IgnoreError=0x0004;
        public static int Pipeline_AutoID_Random=0x0005;
    }
    public enum VersionChannel
    {
        Stable,
        Testing
    }
}
