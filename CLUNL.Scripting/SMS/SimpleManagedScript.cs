﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace CLUNL.Scripting.SMS
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
        ScriptEnvironment Base;
        ScriptEnvironment Current;
        List<SMSSingleCommand> CommandSet = new List<SMSSingleCommand>();
        Dictionary<string, int> Labels = new Dictionary<string, int>();
        Dictionary<string, Data> Memory = new Dictionary<string, Data>();

        public object Eval(string str, out List<ScriptError> result)
        {
            object __result = null;
            Current.Expose("Result", __result);
            result = new List<ScriptError>();
            Compile(str, ref result);
            GatherLabels();
            for (int Index = 0; Index < CommandSet.Count; Index++)
            {
                var current = CommandSet[Index];
                if(SingleExec(ref result, ref Index, current))return null;
            }
            return Current.ExposedObjects["Result"];
        }

        private bool SingleExec(ref List<ScriptError> result, ref int Index, SMSSingleCommand current)
        {
            switch (current.operation)
            {
                case SMSOperation.NEW:
                    {
                        NEW_Operation(current);
                    }
                    break;
                case SMSOperation.NEWT:
                    {
                        NEWT_Operation(current);
                    }
                    break;
                case SMSOperation.SET:
                    {
                        if (SET_Operation(ref result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.SETF:
                    {
                        if (SETF_Operation(result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.EXEC:
                    {
                        if (EXEC_Operation(result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.EXER:
                    {
                        if (EXER_Operation(ref result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.EQL:
                    {
                        result = EQL_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.LGR:
                    {
                        if (LGR_Operation(ref result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.LGE:
                    {
                        if (LGE_Operation(ref result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.IF:
                    {
                        if (IF_Operation(result, ref Index, current)) return true;
                    }
                    break;
                case SMSOperation.J:
                    {
                        if (J_Operation(result, ref Index, current)) return true;
                    }
                    break;
                case SMSOperation.LABEL:
                    break;
                case SMSOperation.END:
                    //End the execution of the script immediately.
                    Index = CommandSet.Count + 1;
                    break;
                case SMSOperation.ENDLABEL:
                    //A mark of the end of previous label.
                    break;
                case SMSOperation.DEL:
                    {
                        DEL_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.ADD:
                    {
                        result = ADD_Opertaion(result, Index, current);
                    }
                    break;
                case SMSOperation.ADDI:

                    {
                        result = ADDI_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.MULT:
                    {
                        result = MULT_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.MULTI:
                    {
                        result = MULTI_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.DIV:
                    {
                        result = DIV_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.DIVI:
                    {
                        result = DIVI_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.DIVII:
                    {
                        result = DIVII_Operation(result, Index, current);
                    }
                    break;
                case SMSOperation.SW:
                    {
                        if (SW_Operation(result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.ADDW:
                    {
                        if (ADDW_Operation(result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.LW:
                    {
                        if (LW_Operation(ref result, Index, current)) return true;
                    }
                    break;
                case SMSOperation.NL:
                    {
                        NL_Operation(current);
                    }
                    break;
                case SMSOperation.ND:
                    {
                        ND_Operation(current);
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        private bool LGE_Operation(ref List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            var obj0 = Parse(current.parameters[0]);
            if (obj0 is IComparable)
            {
                SetObject(current.OperateDatapath, ((IComparable)Parse(current.parameters[0])).CompareTo(Parse(current.parameters[1])) >= 0, typeof(bool), ref result, Index);
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.OBJ_IS_NOT_COMPARABLE, ErrorType = ErrorType.Error, Message = $"Comparison Failed: Target object \"{current.OperateDatapath}\" is not comparable!.", Position = Index });
                return true;
            }
            return false;
        }

        private bool LGR_Operation(ref List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            var obj0 = Parse(current.parameters[0]);
            if (obj0 is IComparable)
            {
                SetObject(current.OperateDatapath, ((IComparable)Parse(current.parameters[0])).CompareTo(Parse(current.parameters[1])) > 0, typeof(bool), ref result, Index);
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.OBJ_IS_NOT_COMPARABLE, ErrorType = ErrorType.Error, Message = $"Comparison Failed: Target object \"{current.OperateDatapath}\" is not comparable!.", Position = Index });
                return true;
            }
            return false;
        }

        private bool IF_Operation(List<ScriptError> result, ref int Index, SMSSingleCommand current)
        {
            if ((bool)FindObject(current.OperateDatapath) == true)
            {
                var i = FindLabel(current.parameters[0]);
                if (i == -1)
                {
                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"Jumper Failed: Target label \"{current.parameters[0]}\" does not exist!.", Position = Index });
                    return true;
                }
                Index = i;
            }
            return false;
        }

        private bool J_Operation(List<ScriptError> result, ref int Index, SMSSingleCommand current)
        {
            var i = FindLabel(current.OperateDatapath);
            if (i == -1)
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"Jumper Failed: Target label \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                Index = -1;
                return true;
            }
            Index = i;
            return false;
        }

        private void DEL_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            if (Memory.ContainsKey(current.OperateDatapath))
            {
                Memory.Remove(current.OperateDatapath);
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Warning, Message = $"Deletion Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
            }
        }

        private bool ADDW_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            object list = FindObject(current.OperateDatapath);
            if (list == null)
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.parameters[0]}\" does not exist!.", Position = Index });
                return true;
            }
            else
            if ((list as IList) != null)
            {
                var l = (list as IList);
                var d = Parse(current.parameters[0]);
                if (d == null)
                {
                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.parameters[0]}\" does not exist!.", Position = Index });
                    return true;
                }
                Trace.WriteLine($"Add {current.parameters[0]} to {current.OperateDatapath}");
                l.Add(d);
            }
            else if ((list as IDictionary) != null)
            {
                var index = Parse(current.parameters[0]);
                var d = (list as IDictionary);
                if (d.Contains(index)) d[index] = Parse(current.parameters[1]);
                else d.Add(index, Parse(current.parameters[1]));
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.INVALID_DATA_TYPE, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.OperateDatapath}\" cannot be casted to IList or IDictionary!.", Position = Index });
                return true;
            }
            return false;
        }

        private bool LW_Operation(ref List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            object list = FindObject(current.OperateDatapath);
            if (list == null)
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.parameters[0]}\" does not exist!.", Position = Index });
                return true;
            }
            else
            if ((list as IList) != null)
            {
                SetObject(current.parameters[1], (list as IList)[Convert.ToInt32(Parse(current.parameters[0]))], null, ref result, Index);
            }
            else if ((list as IDictionary) != null)
            {
                SetObject(current.parameters[1], (list as IDictionary)[Parse(current.parameters[0])], null, ref result, Index);
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.INVALID_DATA_TYPE, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.OperateDatapath}\" cannot be casted to IList or IDictionary!.", Position = Index });
                return true;
            }
            return false;
        }

        private List<ScriptError> EQL_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            var obj0 = Parse(current.parameters[0]);
            if (obj0 is IComparable)
            {
                SetObject(current.OperateDatapath, ((IComparable)Parse(current.parameters[0])).CompareTo(Parse(current.parameters[1])) == 0, typeof(bool), ref result, Index);
            }
            else
                SetObject(current.OperateDatapath, Parse(current.parameters[0]).Equals(Parse(current.parameters[1])), typeof(bool), ref result, Index);
            return result;
        }

        private bool EXER_Operation(ref List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            object ReferencedTarget;
            Type ReferencedType = null;
            ReferencedTarget = FindObject(current.OperateDatapath);
            if (ReferencedTarget == null)
            {
                ReferencedType = FindType(current.OperateDatapath);
                if (ReferencedType == null)
                {
                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"EXEC Failed: Target object or type \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                    return true;
                }
                else
                {
                    object[] parameters_t = null;
                    if (current.parameters.Length > 1)
                    {
                        parameters_t = new object[current.parameters.Length - 1];

                        for (int _i = 2; _i < current.parameters.Length; _i++)
                        {
                            parameters_t[_i - 2] = Parse(current.parameters[_i]);
                        }
                    }
                    Type[] types = ReferencedType == null ? null : new Type[parameters_t.Length];
                    if (parameters_t != null)
                    {
                        for (int i = 0; i < parameters_t.Length; i++)
                        {
                            types[i] = parameters_t.GetType();
                        }
                    }
                    var m_t = ReferencedType.GetMethod(current.parameters[1], types);
                    SetObject(current.parameters[0], m_t.Invoke(null, parameters_t), null, ref result, Index);
                    return false;
                }
            }
            {
                object[] parameters = null;
                if (current.parameters.Length > 1)
                {
                    parameters = new object[current.parameters.Length - 1];

                    for (int _i = 2; _i < current.parameters.Length; _i++)
                    {
                        parameters[_i - 2] = Parse(current.parameters[_i]);
                    }
                }
                Type[] types = ReferencedType == null ? null : new Type[parameters.Length];
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        types[i] = parameters.GetType();
                    }
                }
                var m = ReferencedTarget.GetType().GetMethod(current.parameters[1], types);
                SetObject(current.parameters[0], m.Invoke(ReferencedTarget, parameters), null, ref result, Index);
            }
            return false;
        }

        private bool SW_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            object list = FindObject(current.OperateDatapath);
            if (list == null)
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.parameters[0]}\" does not exist!.", Position = Index });
                return true;
            }
            else
            if ((list as IList) != null)
            {
                (list as IList)[Convert.ToInt32(Parse(current.parameters[0]))] = Parse(current.parameters[1]);
            }
            else if ((list as IDictionary) != null)
            {
                (list as IDictionary)[Parse(current.parameters[0])] = Parse(current.parameters[1]);
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.INVALID_DATA_TYPE, ErrorType = ErrorType.Error, Message = $"ADDW Failed: Target object \"{current.OperateDatapath}\" cannot be casted to IList or IDictionary!.", Position = Index });
                return true;
            }
            return false;
        }

        private bool EXEC_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            object ReferencedTarget;
            Type ReferencedType = null;
            ReferencedTarget = FindObject(current.OperateDatapath);
            if (ReferencedTarget == null)
            {
                ReferencedType = FindType(current.OperateDatapath);
                if (ReferencedType == null)
                {
                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"EXEC Failed: Target object or type \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                    return true;
                }
                else
                {
                    object[] parameters_t = null;
                    if (current.parameters.Length > 1)
                    {
                        parameters_t = new object[current.parameters.Length - 1];

                        for (int _i = 1; _i < current.parameters.Length; _i++)
                        {
                            parameters_t[_i - 1] = Parse(current.parameters[_i]);
                        }
                    }
                    Type[] types = ReferencedType == null ? null : new Type[parameters_t.Length];
                    if (parameters_t != null)
                    {
                        for (int i = 0; i < parameters_t.Length; i++)
                        {
                            types[i] = parameters_t.GetType();
                        }
                    }
                    var m_t = ReferencedType.GetMethod(current.parameters[0], types);
                    m_t.Invoke(null, parameters_t);
                    return false;
                }
            }
            {
                object[] parameters = null;
                if (current.parameters.Length > 1)
                {
                    parameters = new object[current.parameters.Length - 1];
                    for (int _i = 1; _i < current.parameters.Length; _i++)
                    {
                        parameters[_i - 1] = Parse(current.parameters[_i]);
                    }
                }
                Type[] types = ReferencedType == null ? null : new Type[parameters.Length];
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        types[i] = parameters.GetType();
                    }
                }
                var m = ReferencedTarget.GetType().GetMethod(current.parameters[0], types);
                m.Invoke(ReferencedTarget, parameters);
            }
            return false;
        }

        private bool SETF_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            FieldInfo FinalField = null;
            object TargetObject = FindObject(current.OperateDatapath);
            Type t = TargetObject.GetType();
            for (int i = 0; i < current.parameters.Length - 1; i++)
            {
                FinalField = t.GetField(current.parameters[i]);
                if (FinalField != null)
                {
                    TargetObject = FinalField.GetValue(TargetObject);
                    t = FinalField.ReflectedType;
                }
                else
                {
                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.MEMBER_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"SETF Failed: Target member \"{current.OperateDatapath} ->...-> {current.parameters[i]}\" does not exist!.", Position = Index });
                    return true;
                }
            }
            if (FinalField != null)
                FinalField.SetValue(TargetObject, Parse(current.parameters.Last()));
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.MEMBER_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"SETF Failed: Target member \"{current.OperateDatapath} ->...-> null\" does not exist!.", Position = Index });
                return true;
            }
            return false;
        }

        private bool SET_Operation(ref List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            if (SetObject(current.OperateDatapath, Parse(current.parameters[0]), null, ref result, Index))
            { }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"SET Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                return true;
            }
            return false;
        }

        private void ND_Operation(SMSSingleCommand current)
        {
            var t = typeof(List<int>).Assembly.GetType($"System.Collections.Generic.Dictionary`2[{FindType(current.parameters[0])},{FindType(current.parameters[1])}]");
            Data d = new Data();
            d.DataType = t;
            d.CoreData = Activator.CreateInstance(t);
            if (!Memory.ContainsKey(current.OperateDatapath))
                Memory.Add(current.OperateDatapath, d);
            else Memory[current.OperateDatapath] = d;
        }

        private void NL_Operation(SMSSingleCommand current)
        {
            var t = typeof(List<int>).Assembly.GetType($"System.Collections.Generic.List`1[{FindType(current.parameters[0])}]");
            Data d = new Data();
            d.DataType = t;
            d.CoreData = Activator.CreateInstance(t);
            if (!Memory.ContainsKey(current.OperateDatapath))
                Memory.Add(current.OperateDatapath, d);
            else Memory[current.OperateDatapath] = d;
        }

        private List<ScriptError> DIVII_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[2]);
            Number n1 = new Number(null, current.parameters[1], t);
            Number n2 = new Number(obj1, current.parameters[2], t);
            SetObject(Target, n1 / n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> DIVI_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(null, current.parameters[2], t);
            SetObject(Target, n1 / n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> DIV_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            object obj2 = FindObject(current.parameters[2]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(obj2, current.parameters[2], t);
            SetObject(Target, n1 / n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> MULTI_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(null, current.parameters[2], t);
            SetObject(Target, n1 * n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> MULT_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            object obj2 = FindObject(current.parameters[2]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(obj2, current.parameters[2], t);
            SetObject(Target, n1 * n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> ADDI_Operation(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(null, current.parameters[2], t);
            SetObject(Target, n1 + n2, null, ref result, Index);
            return result;
        }

        private List<ScriptError> ADD_Opertaion(List<ScriptError> result, int Index, SMSSingleCommand current)
        {
            Type t = FindType(current.parameters[0]);
            string Target = current.OperateDatapath;
            object obj1 = FindObject(current.parameters[1]);
            object obj2 = FindObject(current.parameters[2]);
            Number n1 = new Number(obj1, current.parameters[1], t);
            Number n2 = new Number(obj2, current.parameters[2], t);
            SetObject(Target, n1 + n2, null, ref result, Index);
            return result;
        }

        private void NEWT_Operation(SMSSingleCommand current)
        {
            Data d = new Data();
            d.CoreData = FindType(current.parameters[0]);
            d.DataType = typeof(Type);
            if (!Memory.ContainsKey(current.OperateDatapath))
                Memory.Add(current.OperateDatapath, d);
            else Memory[current.OperateDatapath] = d;
        }

        private void NEW_Operation(SMSSingleCommand current)
        {
            Data d = new Data();
            d.DataType = FindType(current.parameters[0]);
            if (current.parameters.Length > 1)
            {
                object[] parameters = new object[current.parameters.Length - 1];

                for (int _i = 1; _i < current.parameters.Length; _i++)
                {
                    parameters[_i - 1] = Parse(current.parameters[_i]);
                }
                d.CoreData = Activator.CreateInstance(d.DataType, parameters);
            }
            else
            {
                d.CoreData = Activator.CreateInstance(d.DataType);
            }
            if (!Memory.ContainsKey(current.OperateDatapath))
                Memory.Add(current.OperateDatapath, d);
            else Memory[current.OperateDatapath] = d;
        }

        public async Task<(object, List<ScriptError>)> EvalAsync(string str)
        {
            List<ScriptError> result = null;
            object _result = null;
            await Task.Run(() => { _result = Eval(str, out result); });
            return (_result, result);
        }
        public object Parse(string SrcObj)
        {
            if (SrcObj.IndexOf(":") > 0)
            {
                string t = SrcObj.Substring(0, SrcObj.IndexOf(":"));
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
                    case "T":
                        return FindType(content);
                    case "E":
                        {
                            return FindObject(content);
                        }
                    default:
                        Type TargetT = FindType(t);
                        if (TargetT == null) return null;
                        var c = TypeDescriptor.GetConverter(TargetT);
                        return c.ConvertFromString(content);
                }
            }
            return null;
        }
        public bool SetObject(string name, object data, Type t, ref List<ScriptError> result, int Index)
        {
            if (Current.ExposedObjects.ContainsKey(name))
            {
                Current.ExposedObjects[name] = data;

            }
            else
            if (Memory.ContainsKey(name))
            {
                var d = Memory[name];
                d.CoreData = data;
                Memory[name] = d;
            }
            else
            {
                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"SET Failed: Target reference \"{name}\" does not exist!.", Position = Index });
                return false;
            }
            return true;
        }
        public object FindObject(string Name)
        {
            if (Current.ExposedObjects.ContainsKey(Name))
            {
                return Current.ExposedObjects[Name];
            }
            else if (Memory.ContainsKey(Name))
            {
                return Memory[Name].CoreData;
            }
            return null;
        }
        public Type FindType(string Name)
        {
            switch (Name)
            {
                case "Int":
                    return typeof(int);
                case "Float":
                    return typeof(float);
                case "Boolean":
                case "Bool":
                    return typeof(bool);
                case "String":
                    return typeof(string);
                case "Double":
                    return typeof(double);
                case "Byte":
                    return typeof(byte);
                case "UInt":
                    return typeof(uint);
                case "Long":
                    return typeof(long);
                case "BigInt":
                    return typeof(BigInteger);
            }
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
            CommandSet.Clear();
            StringReader stringReader = new StringReader(Content);
            string Line = null;
            while ((Line = stringReader.ReadLine()) != null)
            {
                Line = Line.Trim();

                if (!Line.StartsWith("#"))
                    if (!Line.StartsWith(";"))
                    {
                        if (Line == "") continue;
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
                                    if (command.OperateDatapath != null && command.OperateDatapath != "")
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
                        CommandSet.Add(command);
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
        public int FindLabel(string Name)
        {
            if (Labels.ContainsKey(Name)) return Labels[Name];
            else return -1;
        }
        public void GatherLabels()
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
        public ScriptEnvironment GetCurrentEnvironment()
        => Current;

        public Dictionary<string, Data> GetMemory()
        {
            return Memory;
        }
        public void ClearMemory()
        {
            Memory.Clear();
        }
        public void ResetEngine()
        {
            Current.Dispose();
            Current = Base.HardCopy();
            Memory.Clear();
        }

        public void SetEnvironmentBase(ScriptEnvironment environment)
        {
            Base = environment;
            Current = Base.HardCopy();

        }

        public void Dispose()
        {
            Current.Dispose();
            ClearMemory();
            CommandSet.Clear();
            Base = null;
        }
    }
}
