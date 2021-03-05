using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Scripting
{
    public class SimpleManagedScript : IEngine
    {
        Environment Base;
        Environment Current;
        List<SMSSingleCommand> CommandSet = new List<SMSSingleCommand>();
        Dictionary<string, int> Labels = new Dictionary<string, int>();
        Dictionary<string, Data> Memory = new Dictionary<string, Data>();
        public void Eval(string str)
        {

            throw new NotImplementedException();
        }

        public Environment GetCurrentEnvironment()
        => Current;

        public Dictionary<string,Data> GetMemory()
        {
            return Memory;
        }
        public void ResetEngine()
        {
            Current.Dispose();
            Current = Base.HardCopy();
            Memory.Clear();
        }

        public void SetEnvironmentBase(Environment environment)
        {
            Base = environment;
            Current = Base.HardCopy();
        }
    }
    internal struct Data
    {
        object CoreData;
        Type DataType;
    }
    internal enum SMSOperation
    {
        NEW,SET,EXEC,IF,J,LABEL,END,ENDLABEL,
    }
    internal struct SMSSingleCommand
    {
        SMSOperation operation;
        string OperateDatapath;
    }
}
