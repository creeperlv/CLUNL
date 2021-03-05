using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Scripting
{
    public interface IEngine
    {
        Environment GetCurrentEnvironment();
        void SetEnvironmentBase(Environment environment);
        void Eval(string str);
        void ResetEngine();
    }
}
