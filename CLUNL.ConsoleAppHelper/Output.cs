using CLUNL.Localization;
namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Represent output of the console application.
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// Output an object.
        /// </summary>
        /// <param name="o"></param>
        public static void Out(object o)
        {
            ConsoleAppHelper.Out(o);
        }
        /// <summary>
        /// Output a message with given language string ID and fallback with optional.
        /// </summary>
        /// <param name="stringID"></param>
        /// <param name="Fallback"></param>
        /// <param name="parameters"></param>
        public static void Out(string stringID, string Fallback, params object[] parameters)
        {
            ConsoleAppHelper.Out(Language.FindF(stringID, Fallback, parameters));
        }
        /// <summary>
        /// Output an error message.
        /// </summary>
        /// <param name="msg"></param>
        public static void Out(ErrorMsg msg)
        {
            ConsoleAppHelper.Out(msg);
        }
        /// <summary>
        /// Output an error message, followed by the current line terminator.
        /// </summary>
        /// <param name="msg"></param>
        public static void OutLine(ErrorMsg msg)
        {
            ConsoleAppHelper.OutLine(msg);
        }
        /// <summary>
        /// Output a warning message.
        /// </summary>
        /// <param name="msg"></param>
        public static void Out(WarnMsg msg)
        {
            ConsoleAppHelper.Out(msg);
        }
        /// <summary>
        /// Output a warning message, followed by the current line terminator.
        /// </summary>
        /// <param name="msg"></param>
        public static void OutLine(WarnMsg msg)
        {
            ConsoleAppHelper.OutLine(msg);
        }
        /// <summary>
        /// Output a blank line.
        /// </summary>
        public static void OutLine()
        {
            ConsoleAppHelper.OutLine();
        }
        /// <summary>
        /// OUtput on object, followed by the current line terminator.
        /// </summary>
        /// <param name="o"></param>
        public static void OutLine(object o)
        {
            ConsoleAppHelper.OutLine(o);
        }
        /// <summary>
        /// Output a message with given language string ID and fallback with optional, followed by the current line terminator.
        /// </summary>
        /// <param name="stringID"></param>
        /// <param name="Fallback"></param>
        /// <param name="parameters"></param>
        public static void OutLine(string stringID, string Fallback, params object[] parameters)
        {
            ConsoleAppHelper.OutLine(Language.FindF(stringID, Fallback, parameters));
        }
        /// <summary>
        /// Output two object in format of "`o1` : `o1`", followed by the current line terminator.
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public static void OutLine(object o1, object o2)
        {
            ConsoleAppHelper.OutLine(o1, o2);
        }
    }
}
