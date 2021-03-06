using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Reflection;
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
        List<string> UsingNamespaces = new List<string>();
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
                    case "Byte":
                        return byte.Parse(content);
                    case "UInt":
                        return uint.Parse(content);
                    case "Long":
                        return long.Parse(content);
                    case "BigInt":
                        return BigInteger.Parse(content);
                    case "FileInfo":
                        return new FileInfo(content);
                    case "DirectoryInfo":
                        return new DirectoryInfo(content);
                    case "E":
                        {
                            foreach (var ObjectItem in Current.ExposedObjects)
                            {
                                if (ObjectItem.Key == content)
                                    return ObjectItem.Value;
                            }
                            foreach (var item in Memory)
                            {
                                if (item.Key == content)
                                    return item.Value;
                            }
                        }
                        break;
                    default:
                        Type TargetT = FindType(t);
                        if (TargetT == null) return null;
                        var c = TypeDescriptor.GetConverter(TargetT);
                        return c.ConvertFromString(content);
                }
            }
            return null;
        }
        public Type FindType(string Name)
        {
            if (Name.IndexOf(".") > -1)
            {
                foreach (var item in Current.ExposedTypes)
                {
                    if (item.Value.Name == Name) return item.Value;
                }
#if SMS_FIND_ALL_TYPES

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    return assembly.GetType(Name);
                }
#endif
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    return assembly.GetType(Name);
                }
            }
            var RName = Name;
            if (!RName.StartsWith(".")) RName = "." + RName;
            foreach (var item in Current.ExposedTypes)
            {
                if (item.Key == Name) return item.Value;
                if (item.Value.Name.EndsWith(RName)) return item.Value;
            }
#if SMS_FIND_ALL_TYPES
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name.EndsWith(RName))
                    {
                        return type;
                    }
                }
            }
#endif
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
                        command.operation = Op;
                        command.OperateDatapath = CMDs[1];
                        CMDs.RemoveAt(0);
                        CMDs.RemoveAt(0);
                        command.parameters = CMDs.ToArray();
                        {
                            //Special Check.
                            switch (Op)
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
                                    break;
                                case SMSOperation.LABEL:
                                    break;
                                case SMSOperation.END:
                                    if (command.OperateDatapath != null&& command.OperateDatapath != "")
                                    {
                                        errors.Add(new ScriptError() { ErrorType = ErrorType.Error, ErrorTime = ErrorTime.Compile, ID = ScriptErrorIDs.WRONG_SYNATX, Message = "END command should have no parameters." });
                                    }
                                    if (command.parameters.Length != 0)
                                    {
                                        errors.Add(new ScriptError() { ErrorType = ErrorType.Error, ErrorTime = ErrorTime.Compile, ID = ScriptErrorIDs.WRONG_SYNATX, Message = "END command should have no parameters." });
                                    }
                                    break;
                                case SMSOperation.ENDLABEL:
                                    if (command.OperateDatapath != null && command.OperateDatapath != "")
                                    {
                                        errors.Add(new ScriptError() { ErrorType = ErrorType.Error, ErrorTime = ErrorTime.Compile, ID = ScriptErrorIDs.WRONG_SYNATX, Message = "ENDLABEL command should have no parameters." });
                                    }
                                    if (command.parameters.Length != 0)
                                    {
                                        errors.Add(new ScriptError() { ErrorType = ErrorType.Error, ErrorTime = ErrorTime.Compile, ID = ScriptErrorIDs.WRONG_SYNATX, Message = "ENDLABEL command should have no parameters." });
                                    }
                                    break;
                                case SMSOperation.DEL:
                                    break;
                                case SMSOperation.ADD:
                                    break;
                                case SMSOperation.ADDI:
                                    break;
                                case SMSOperation.MULT:
                                    break;
                                case SMSOperation.DIV:
                                    break;
                                default:
                                    break;
                            }
                        }
                        //switch (Op)
                        //{
                        //    case SMSOperation.NEW:
                        //        {
                        //            command.OperateDatapath = CMDs[1];
                        //        }
                        //        break;
                        //    case SMSOperation.SET:
                        //        break;
                        //    case SMSOperation.EXEC:
                        //        break;
                        //    case SMSOperation.IF:
                        //        break;
                        //    case SMSOperation.J:
                        //        break;
                        //    case SMSOperation.LABEL:
                        //        break;
                        //    case SMSOperation.END:
                        //        break;
                        //    case SMSOperation.ENDLABEL:
                        //        break;
                        //    case SMSOperation.DEL:
                        //        break;
                        //    default:
                        //        break;
                        //}
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
                    case SMSOperation.LABEL:
                        {
                            Labels.Add(command.OperateDatapath, i);
                        }
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
        NEW = 0x00,             //Create new object.
        SET = 0x01,             //Set value to an object.
        EXEC = 0x02,            //Execute external method.
        IF = 0x03,              //If sentence.
        J = 0x04,               //Jump to label.
        LABEL = 0x05,           //Define label.
        END = 0x06,             //End of program.
        ENDLABEL = 0x07,        //End of label.
        DEL = 0x08,             //Delete object.
        ADD = 0x09,             //Add Object0 = Object1 + Object2.  ADD TYPE OBJ0 OBJ1 OBJ2
        ADDI = 0x0A,            //Add immediately. ADD TYPE OBJ0 OBJ1 NUMBER
        MULT = 0x0B,            //Multiply Object0=Object1*Object2. MULT TYPE OBJ0 OBJ1 OBJ2
        MULTI=0x0C,             //Multiply immediately. MULT TYPE OBJ0 OBJ1 NUMBER
        DIV = 0x0D,             //Divide Object0=Object1/Object2. DIV TYPE OBJ0 OBJ1 OBJ2
        DIVI = 0x0E,            //Divide immediately. DIVI TYPE OBJ0 OBJ1 NUMBER
        DIVII=0x0F,             //Divide inversed immediately. DIVI TYPE OBJ0 NUMBER OBJ1
        SW=0x10,                //Save Word. SW ARRAY_OBJECT INDEX TYPE:NUMBER
        ADDW=0x11,              //Add word to ArrayList. ADDW LIST_OBJECT TYPE:DATA                        
        LW=0x12,                //Load word. LW ARRAY_OBJECT INDEX TARGET_OBJECT
    }
    internal struct SMSSingleCommand
    {
        internal SMSOperation operation;
        internal string OperateDatapath;
        internal string[] parameters;
    }
}
