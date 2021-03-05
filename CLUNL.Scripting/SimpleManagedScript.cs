using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public Task EvalAsync(string str)
        {
            throw new NotImplementedException();
        }

        public void ExecuteLoadedCommandSet()
        {
            for (int i = 0; i < CommandSet.Count; i++)
            {
                var command = CommandSet[i];
                switch (command.operation)
                {
                    case SMSOperation.NEW:
                        break;
                    case SMSOperation.SET:
                        break;
                    case SMSOperation.EXEC:
                        break;
                    case SMSOperation.IF:
                        break;
                    case SMSOperation.J:
                        {
                            if (Labels.ContainsKey(command.OperateDatapath))
                                i = Labels[command.OperateDatapath];
                            else
                            {

                            }
                        }
                        break;
                    case SMSOperation.LABEL:
                        break;
                    case SMSOperation.END:
                        break;
                    case SMSOperation.ENDLABEL:
                        break;
                    default:
                        break;
                }
            }
        }
        public Environment GetCurrentEnvironment()
        => Current;

        public Dictionary<string, Data> GetMemory()
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

        Task IEngine.Eval(string str)
        {
            throw new NotImplementedException();
        }
    }
    public struct Data
    {
        public object CoreData;
        public Type DataType;
    }
    internal enum SMSOperation
    {
        NEW = 0x00, SET = 0x01, EXEC = 0x02, IF = 0x03, J = 0x04, LABEL = 0x05, END = 0x06, ENDLABEL = 0x07, DEL = 0x08
    }
    internal struct SMSSingleCommand
    {
        internal SMSOperation operation;

        internal string OperateDatapath;
        internal string Op1;
        internal string Op2;
    }
}
