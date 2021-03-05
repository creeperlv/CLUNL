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
    public struct ScriptError
    {
        public ErrorType ErrorType;
        public ErrorTime ErrorTime;
        public int Position;
        public int ID;
        public string Message;
    }
}
