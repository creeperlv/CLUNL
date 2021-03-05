using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.Scripting
{
    /*
     * SMS Goal Script:
     * NEW Object1 Int 1
     * SET Object1 Int:1
     * NEW StringObj String "Content"
     * SET StringObj String:"Content2"
     */
    public class SimpleManagedScript : IEngine
    {
        Environment Base;
        Environment Current;
        List<SMSSingleCommand> CommandSet = new List<SMSSingleCommand>();
        Dictionary<string, int> Labels = new Dictionary<string, int>();
        Dictionary<string, Data> Memory = new Dictionary<string, Data>();
        public List<ScriptError> Eval(string str)
        {

            List<ScriptError> result = new List<ScriptError>();
            Compile(str, ref result);
            throw new NotImplementedException();
        }

        public async Task<List<ScriptError>> EvalAsync(string str)
        {
            List<ScriptError> result = null;
            await Task.Run(() => { result = Eval(str); });
            return result;
        }
        public object Parse(string SrcObj)
        {
            if (SrcObj.IndexOf(":") > 0)
            {
                string t = SrcObj.Substring(SrcObj.IndexOf(":"));
                string content = SrcObj.Substring(SrcObj.IndexOf(":") + 1);
                switch (t)
                {
                    case "Int":
                        return int.Parse(content);
                    case "Float":
                        return float.Parse(content);
                    case "Boolean":
                    case "Bool":
                        return bool.Parse(content);
                    case "String":
                        return content;
                    case "Double":
                        return double.Parse(content);

                    default:
                        TypeConverter typeConverter = new TypeConverter();
                        return typeConverter.ConvertFromString( null,SrcObj);
                        break;
                }
            }
            return null;
        }
        public void Compile(string Content, ref List<ScriptError> errors)
        {
            StringReader stringReader = new StringReader(Content);
            string Line = null;
            while ((Line = stringReader.ReadLine()) != null)
            {
                Line = Line.Trim();

                if (!Line.StartsWith("#"))
                    if (!Line.StartsWith(";"))
                    {
                        SMSSingleCommand command = new SMSSingleCommand();
                        var CMDs = Utilities.ResolveParameters(Line);
                        var Operator = CMDs[0];
                        var Op = (SMSOperation)Enum.Parse(typeof(SMSOperation), Operator);
                        switch (Op)
                        {
                            case SMSOperation.NEW:
                                {
                                    command.OperateDatapath = CMDs[1];
                                }
                                break;
                            case SMSOperation.SET:
                                break;
                            case SMSOperation.EXEC:
                                break;
                            case SMSOperation.IF:
                                break;
                            case SMSOperation.J:
                                break;
                            case SMSOperation.LABEL:
                                break;
                            case SMSOperation.END:
                                break;
                            case SMSOperation.ENDLABEL:
                                break;
                            case SMSOperation.DEL:
                                break;
                            default:
                                break;
                        }
                    }
            }
        }
        public void ScanForLabel()
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
                        }
                        break;
                    case SMSOperation.LABEL:
                        {
                            Labels.Add(command.OperateDatapath, i);
                        }
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
        public void ExecuteLoadedCommandSet(ref List<ScriptError> errors)
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
                                errors.Add(new ScriptError() { ErrorType = ErrorType.Error, ID = 0, ErrorTime = ErrorTime.Execute, Position = i, Message = $"Cannot find label:{command.OperateDatapath} (At Command {i})" });
                                return;
                            }
                        }
                        break;
                    case SMSOperation.LABEL:
                        {

                        }
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
    }
    public struct Data
    {
        public object CoreData;
        public Type DataType;
    }
    internal enum SMSOperation
    {
        NEW = 0x00, SET = 0x01, EXEC = 0x02, IF = 0x03, J = 0x04, LABEL = 0x05, END = 0x06, ENDLABEL = 0x07, DEL = 0x08, ADD = 0x09, ADDI = 0x0A, MULT = 0x0B, DIV = 0x0C,USE=0x0D
    }
    internal struct SMSSingleCommand
    {
        internal SMSOperation operation;
        internal string OperateDatapath;
        internal string[] parameters;
    }
}
