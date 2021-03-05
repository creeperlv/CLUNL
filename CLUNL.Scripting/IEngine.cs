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
        void Eval(string str);
        Task EvalAsync(string str);
        void ResetEngine();
        Dictionary<string, Data> GetMemory();
    }
}
