using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.Diagnostics
{
    /// <summary>
    /// Logger
    /// </summary>
    public class Logger
    {
        static bool __init=false;
        static List<ILogger> Loggers = new List<ILogger>();
        /// <summary>
        /// Init the logger, call in the very early stage of the program.
        /// </summary>
        public static void Init()
        {
            if (__init) return;
            __init = true;
            RegisterLogger(new DefaultConsoleLogger());
            RegisterLogger(new DefaultTraceLogger());
        }
        /// <summary>
        /// Unregister a logger.
        /// </summary>
        /// <param name="L"></param>
        public static void UnregisterLogger(ILogger L)
        {
            lock (Loggers)
            {
                Loggers.Remove(L);
            }
        }
        /// <summary>
        /// Register a logger.
        /// </summary>
        /// <param name="l"></param>
        public static void RegisterLogger(ILogger l)
        {
            lock (Loggers)
            {
                Loggers.Add(l);
            }
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="o"></param>
        public static void WriteLine(object o)
        {
            foreach (var item in Loggers)
            {
                item.WriteLine(o);
            }
        }
        /// <summary>
        /// Write an object.
        /// </summary>
        /// <param name="o"></param>
        public static void Write(object o)
        {
            foreach (var item in Loggers)
            {
                item.Write(o);
            }
        }
        /// <summary>
        /// Write two objects in format of "o0: o1".
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public static void Write(object o0, object o1)
        {
            foreach (var item in Loggers)
            {
                item.Write(o0,o1);
            }
        }
        /// <summary>
        /// Write a line in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public static void WriteLine(object o0, object o1)
        {
            foreach (var item in Loggers)
            {
                item.WriteLine(o0, o1);
            }
        }
        /// <summary>
         /// Write a line in format of "o0: o1"
         /// </summary>
         /// <param name="ML"></param>
         /// <param name="o0"></param>
         /// <param name="o1"></param>
        public static void WriteLine(MessageLevel ML, object o0, object o1)
        {
            foreach (var item in Loggers)
            {
                item.WriteLine(ML, o0, o1);
            }
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        public static void WriteLine(MessageLevel ML, object o)
        {
            foreach (var item in Loggers)
            {
                item.WriteLine(ML, o);
            }
        }
    }
    /// <summary>
    /// Default logger, write to trace.
    /// </summary>
    public class DefaultTraceLogger : ILogger
    {
        /// <summary>
        /// Write a message.
        /// </summary>
        /// <param name="o"></param>
        public void Write(object o)
        {
            Write(MessageLevel.Normal, o);
        }
        /// <summary>
        /// Write a message.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        public void Write(MessageLevel ML, object o)
        {
            Trace.Write("[");
            WriteMessageLevel(ML);
            Trace.Write("]");
            Trace.Write(o);
        }

        private static void WriteMessageLevel(MessageLevel ML)
        {
            switch (ML)
            {
                case MessageLevel.Normal:
                    Trace.Write("NORMAL");
                    break;
                case MessageLevel.Warn:
                    Trace.Write("WARN");
                    break;
                case MessageLevel.Error:
                    Trace.Write("ERROR");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Write two objects in form of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void Write(object o0, object o1)
        {
            Write(MessageLevel.Normal, o0, o1);
        }

        /// <summary>
        /// Write two objects in form of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void Write(MessageLevel ML, object o0, object o1)
        {
            Trace.Write("[");
            WriteMessageLevel(ML);
            Trace.Write("]");
            Trace.Write(o0);
            Trace.Write(": ");
            Trace.Write(o1);
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="o"></param>
        public void WriteLine(object o)
        {
            WriteLine(MessageLevel.Normal, o);
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        public void WriteLine(MessageLevel ML, object o)
        {
            Trace.Write("[");
            WriteMessageLevel(ML);
            Trace.Write("]");
            Trace.WriteLine(o);

        }
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void WriteLine(object o0, object o1)
        {
            WriteLine(MessageLevel.Normal, o0, o1);
        }
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void WriteLine(MessageLevel ML, object o0, object o1)
        {
            Trace.Write("[");
            WriteMessageLevel(ML);
            Trace.Write("]");
            Trace.Write(o0);
            Trace.Write(": ");
            Trace.WriteLine(o1);
        }
    }
    /// <summary>
    /// Default logger, write to console.
    /// </summary>
    public class DefaultConsoleLogger : ILogger
    {
        /// <summary>
        /// Write a message.
        /// </summary>
        /// <param name="o"></param>
        public void Write(object o)
        {
            Write(MessageLevel.Normal, o);
        }
        /// <summary>
        /// Write a message.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        public void Write(MessageLevel ML, object o)
        {
            Console.Write("[");
            WriteMessageLevel(ML);
            Console.Write("]");
            Console.Write(o);
        }
        
        private static void WriteMessageLevel(MessageLevel ML)
        {
            switch (ML)
            {
                case MessageLevel.Normal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("NORMAL");
                    break;
                case MessageLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARN");
                    break;
                case MessageLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR");
                    break;
                default:
                    break;
            }
            Console.ResetColor();
        }
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void Write(object o0, object o1)
        {
            Write(MessageLevel.Normal, o0, o1);
        }
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void Write(MessageLevel ML, object o0, object o1)
        {
            Console.Write("[");
            WriteMessageLevel(ML);
            Console.Write("]");
            Console.Write(o0);
            Console.Write(": ");
            Console.Write(o1);
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="o"></param>
        public void WriteLine(object o)
        {
            WriteLine(MessageLevel.Normal, o);
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        public void WriteLine(MessageLevel ML, object o)
        {
            Console.Write("[");
            WriteMessageLevel(ML);
            Console.Write("]");
            Console.WriteLine(o);

        }

        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void WriteLine(object o0, object o1)
        {
            WriteLine(MessageLevel.Normal, o0, o1);
        }
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        public void WriteLine(MessageLevel ML, object o0, object o1)
        {
            Console.Write("[");
            WriteMessageLevel(ML);
            Console.Write("]");
            Console.Write(o0);
            Console.Write(": ");
            Console.WriteLine(o1);
        }
    }
    /// <summary>
    /// Interface of loggers.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="o"></param>
        void Write(object o);
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        void Write(MessageLevel ML, object o);
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        void Write(object o0, object o1);
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        void Write(MessageLevel ML, object o0, object o1);
        /// <summary>
        /// Write an object.
        /// </summary>
        /// <param name="o"></param>
        void WriteLine(object o);
        /// <summary>
        /// Write an object.
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o"></param>
        void WriteLine(MessageLevel ML, object o);
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        void WriteLine(object o0, object o1);
        /// <summary>
        /// Write two object in format of "o0: o1"
        /// </summary>
        /// <param name="ML"></param>
        /// <param name="o0"></param>
        /// <param name="o1"></param>
        void WriteLine(MessageLevel ML, object o0, object o1);

    }
    /// <summary>
    /// Level of message.
    /// </summary>
    public enum MessageLevel
    {
        /// <summary>
        /// A normal message.
        /// </summary>
        Normal,
        /// <summary>
        /// A warning message.
        /// </summary>
        Warn,
        /// <summary>
        /// An error message.
        /// </summary>
        Error
    }
}
