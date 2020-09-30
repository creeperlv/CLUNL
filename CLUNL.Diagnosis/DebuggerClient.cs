using System;

namespace CLUNL.Diagnosis
{
    /// <summary>
    /// Client of debugger
    /// </summary>
    public class DebuggerClient
    {
        DebuggerClientProfile profile;
        bool isDisposed = false;
        public DebuggerClientProfile Profile { get => profile; }
        /// <summary>
        /// The only public client.
        /// </summary>
        public static DebuggerClient CurrentClient;
        /// <summary>
        /// Make sure only one Client at one time.
        /// </summary>
        /// <param name="clientProfile"></param>
        /// <param name="Path"></param>
        public static void Init(DebuggerClientProfile clientProfile, string Path)
        {
            if (CurrentClient != null)
                if (CurrentClient.isDisposed == true)
                    CurrentClient.Dispose();
            CurrentClient = new DebuggerClient();
            CurrentClient.profile = clientProfile;
            CurrentClient.Init(Path);
        }
        /// <summary>
        /// Real initializer.
        /// </summary>
        /// <param name="Path"></param>
        private void Init(string Path)
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

        private void Dispose()
        {
            throw new NotImplementedException();
        }
        public void Log(string Message, LogLevel logLevel= LogLevel.Default)
        {
            String FinalMessage = "";
            switch (logLevel)
            {
                case LogLevel.Default:
                    FinalMessage += "D[";
                    break;
                case LogLevel.Normal:
                    FinalMessage += "N[";
                    break;
                case LogLevel.Warning:
                    FinalMessage += "W[";
                    break;
                case LogLevel.Error:
                    FinalMessage += "E[";
                    break;
                case LogLevel.Important:
                    FinalMessage += "I[";
                    break;
                default:
                    break;
            }
            FinalMessage += Message;
        }
    }
    public enum LogLevel
    {
        Default,Normal,Warning,Error,Important
    }
    public enum DebuggerClientProfile
    {
        LocalFile, MappedFile
    }
}
