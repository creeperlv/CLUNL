using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.Scripting
{
    public interface IEngine
    {
        Environment GetCurrentEnvironment();
        void SetEnvironmentBase(Environment environment);
        List<ScriptError> Eval(string str);
        Task<List<ScriptError>> EvalAsync(string str);
        void ResetEngine();
        Dictionary<string, Data> GetMemory();
    }
    public enum ErrorType
    {
        Warning,Error
    }
    public enum ErrorTime
    {
        Compile,Execute
    }
    public static class ScriptErrorIDs
    {
        public readonly static int WRONG_SYNATX = 0x0001;
        public readonly static int REFERENCE_DOES_NOT_EXIST = 0x0002;
        public readonly static int LABEL_DOES_NOT_EXIST = 0x0003;
        public readonly static int NUMBER_CONVERSION_ERROR = 0x0004;
    }
    public struct ScriptError
    {
        public ErrorType ErrorType;
        public ErrorTime ErrorTime;
        public int Position;
        public int ID;
        public string Message;
    }
}
