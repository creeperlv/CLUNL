using CLUNL.DirectedIO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.Diagnosis
{
    /// <summary>
    /// MMF - Memory Mapped File
    /// </summary>
    public class MMFDebuggerWatcher
    {
        MMFWR MMFWR;
        bool isStop=false;
        public void Stop()
        {
            isStop = true;
        }
        /// <summary>
        /// Init the watcher. Will start a thread/task to listen.
        /// </summary>
        /// <param name="WR"></param>
        public MMFDebuggerWatcher(MMFWR WR,LogInfoProfile logInfoProfile= LogInfoProfile.R1)
        {
            MMFWR = WR;
            switch (logInfoProfile)
            {
                case LogInfoProfile.R0:

                    break;
                case LogInfoProfile.R1:
                    Task.Run(() => {
                        string line;
                        while ((line = MMFWR.ReadLine()) != ">S[End")
                        {
                            if (isStop == true)
                            {
                                MMFWR.Dispose();
                                return;
                            }
                            try
                            {
                                var desc = LogDescription.Analysis(line);
                                foreach (var item in Listeners)
                                {
                                    item.Invoke((desc, line));
                                }
                            }
                            catch (Exception)
                            {
                            }
                            if (isStop == true)
                            {
                                MMFWR.Dispose();
                                return;
                            }
                        }
                    });
                    break;
                default:
                    break;
            }
            
        }
        List<Action<(LogDescription, string)>> Listeners;
        /// <summary>
        /// Add a debug listener.
        /// </summary>
        /// <param name="Listener"></param>
        public void AddListener(Action<(LogDescription,string)> Listener)
        {
            lock (Listeners)
            {
                Listeners.Add(Listener);
            }
        }
    }
    public class LogDescription
    {
        public DateTime Time { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public static LogDescription Analysis(string Msg)
        {
            LogDescription logDescription = new LogDescription();
            logDescription.Time=DateTime.Parse(Msg.Substring(0, Msg.IndexOf('>')));
            switch (Msg[Msg.IndexOf('>') + 1])
            {
                case 'D':
                    logDescription.LogLevel = LogLevel.Default;
                    break;
                case 'N':
                    logDescription.LogLevel = LogLevel.Normal;
                    break;
                case 'W':
                    logDescription.LogLevel = LogLevel.Warning;
                    break;
                case 'E':
                    logDescription.LogLevel = LogLevel.Error;
                    break;
                case 'I':
                    logDescription.LogLevel = LogLevel.Important;
                    break;
                default:
                    break;
            }
            return logDescription;
        }
    }
}
