using System;

namespace CLUNL.Diagnosis
{
    public class DebuggerClient
    {
        DebuggerClientProfile profile;
        public DebuggerClientProfile Profile { get=>profile;  }
        public static DebuggerClient CurrentClient;
        public static void Init(DebuggerClientProfile clientProfile,string Path)
        {
            CurrentClient = new DebuggerClient();
            CurrentClient.profile = clientProfile;
            CurrentClient.Init(Path);
        }
        public void Init(string Path)
        {
            switch (profile)
            {
                case DebuggerClientProfile.LocalFile:

                    break;
                case DebuggerClientProfile.MappedFile:
                    break;
                default:
                    break;
            }
        }
    }
    public enum DebuggerClientProfile
    {
        LocalFile,MappedFile
    }
}
