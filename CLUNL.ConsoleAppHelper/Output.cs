using CLUNL.Localization;
namespace CLUNL.ConsoleAppHelper
{
    public static class Output
    {
        public static void Out(object o)
        {
            ConsoleAppHelper.Out(o);
        }
        public static void Out(string stringID, string Fallback, params object[] parameters)
        {
            ConsoleAppHelper.Out(Language.FindF(stringID, Fallback, parameters));
        }
        public static void Out(ErrorMsg msg)
        {
            ConsoleAppHelper.Out(msg);
        }
        public static void OutLine(ErrorMsg msg)
        {
            ConsoleAppHelper.OutLine(msg);
        }
        public static void Out(WarnMsg msg)
        {
            ConsoleAppHelper.Out(msg);
        }
        public static void OutLine(WarnMsg msg)
        {
            ConsoleAppHelper.OutLine(msg);
        }
        public static void OutLine()
        {
            ConsoleAppHelper.OutLine();
        }
        public static void OutLine(object o)
        {
            ConsoleAppHelper.OutLine(o);
        }
        public static void OutLine(string stringID, string Fallback, params object[] parameters)
        {
            ConsoleAppHelper.OutLine(Language.FindF(stringID, Fallback, parameters));
        }
        public static void OutLine(object o1, object o2)
        {
            ConsoleAppHelper.OutLine(o1, o2);
        }
    }
}
