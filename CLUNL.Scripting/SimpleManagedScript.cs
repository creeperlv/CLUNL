using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public object Eval(string str, out List<ScriptError> result)
        {
            object __result = null;
            Current.Expose("Result", __result);
            result = new List<ScriptError>();
            Compile(str, ref result);
            for (int Index = 0; Index < CommandSet.Count; Index++)
            {
                var current = CommandSet[Index];
                switch (current.operation)
                {
                    case SMSOperation.NEW:
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
                        break;
                    case SMSOperation.SET:
                        {
                            if (SetObject(current.OperateDatapath, Parse(current.parameters[0]), null, ref result, Index))
                            { }
                            else
                            {
                                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"SET Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                return null;
                            }
                        }
                        break;
                    case SMSOperation.EXEC:
                        {
                            object ReferencedTarget = null;

                            if (Memory.ContainsKey(current.OperateDatapath))
                            {
                                ReferencedTarget = Memory[current.OperateDatapath].CoreData;
                            }
                            else if (Current.ExposedObjects.ContainsKey(current.OperateDatapath))
                            {
                                ReferencedTarget = Current.ExposedObjects[current.OperateDatapath];
                            }
                            if (ReferencedTarget == null)
                            {
                                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"EXEC Failed: Target label \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                return null;
                            }
                            var m = ReferencedTarget.GetType().GetMethod(current.parameters[0]);
                            object[] parameters = null;

                            if (current.parameters.Length > 1)
                            {
                                parameters = new object[current.parameters.Length - 1];

                                for (int _i = 1; _i < current.parameters.Length; _i++)
                                {
                                    parameters[_i - 1] = Parse(current.parameters[_i]);
                                }

                            }
                            m.Invoke(ReferencedTarget, parameters);
                        }
                        break;
                    case SMSOperation.IF:
                        break;
                    case SMSOperation.J:
                        {
                            var i = FindLabel(current.OperateDatapath);
                            if (i == -1)
                            {
                                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.LABEL_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"Jumper Failed: Target label \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                return null;
                            }
                        }
                        break;
                    case SMSOperation.LABEL:
                        break;
                    case SMSOperation.END:
                        //End the execution of the script immediately.
                        return null;
                    case SMSOperation.ENDLABEL:
                        //A mark of the end of previous label.
                        break;
                    case SMSOperation.DEL:
                        {
                            if (Memory.ContainsKey(current.OperateDatapath))
                            {
                                Memory.Remove(current.OperateDatapath);
                            }
                            else
                            {
                                result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"Deletion Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                return null;
                            }
                        }
                        break;
                    case SMSOperation.ADD:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                long i1 = 0;
                                long i2 = 0;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.ADDI:

                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                long i1;
                                long i2;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 + i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.MULT:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                long i1 = 0;
                                long i2 = 0;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.MULTI:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                long i1;
                                long i2;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.DIV:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                object obj2 = FindObject(current.parameters[2]);
                                long i1 = 0;
                                long i2 = 0;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (obj2 != null)
                                {
                                    i2 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 * i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.DIVI:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i1 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i1 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i1 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i1 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                long i1;
                                long i2;
                                if (obj1 != null)
                                {
                                    i1 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.DIVII:
                        {
                            Type t = FindType(current.parameters[0]);
                            String Target = current.OperateDatapath;
                            if (t == typeof(int))
                            {
                                object obj1 = FindObject(current.parameters[2]);
                                int i1;
                                int i2;
                                if (obj1 != null)
                                {
                                    i2 = (int)obj1;
                                }
                                else if (!int.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!int.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(float))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                float i1;
                                float i2;
                                if (obj1 != null)
                                {
                                    i2 = (float)obj1;
                                }
                                else if (!float.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!float.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(byte))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                byte i1;
                                byte i2;
                                if (obj1 != null)
                                {
                                    i2 = (byte)obj1;
                                }
                                else if (!byte.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!byte.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(double))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                double i1;
                                double i2;
                                if (obj1 != null)
                                {
                                    i2 = (double)obj1;
                                }
                                else if (!double.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!double.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                            else if (t == typeof(long))
                            {
                                object obj1 = FindObject(current.parameters[1]);
                                long i1;
                                long i2;
                                if (obj1 != null)
                                {
                                    i2 = (long)obj1;
                                }
                                else if (!long.TryParse(current.parameters[2], out i2))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.REFERENCE_DOES_NOT_EXIST, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target reference \"{current.OperateDatapath}\" does not exist!.", Position = Index });
                                    return null;
                                }
                                if (!long.TryParse(current.parameters[1], out i1))
                                {
                                    result.Add(new ScriptError() { ErrorTime = ErrorTime.Execute, ID = ScriptErrorIDs.NUMBER_CONVERSION_ERROR, ErrorType = ErrorType.Error, Message = $"ADD Failed: Target data \"{current.OperateDatapath}\" is not a number!.", Position = Index });
                                    return null;
                                }
                                SetObject(Target, i1 / i2, null, ref result, Index);
                            }
                        }
                        break;
                    case SMSOperation.SW:
                        break;
                    case SMSOperation.ADDW:
                        break;
                    case SMSOperation.LW:
                        break;
                    default:
                        break;
                }
            }
            return Current.ExposedObjects["Result"];
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
        IF = 0x03,              //If sentence. ID BOOLEANOBJ LABEL
        J = 0x04,               //Jump to label.
        LABEL = 0x05,           //Define label.
        END = 0x06,             //End of program.
        ENDLABEL = 0x07,        //End of label.
        DEL = 0x08,             //Delete object.
        ADD = 0x09,             //Add Object0 = Object1 + Object2.  ADD OBJ0 TYPE OBJ1 OBJ2
        ADDI = 0x0A,            //Add immediately. ADD OBJ0 TYPE OBJ1 NUMBER
        MULT = 0x0B,            //Multiply Object0=Object1*Object2. MULT OBJ0 TYPE OBJ1 OBJ2
        MULTI = 0x0C,             //Multiply immediately. MULT OBJ0 TYPE OBJ1 NUMBER
        DIV = 0x0D,             //Divide Object0=Object1/Object2. DIV OBJ0 TYPE OBJ1 OBJ2
        DIVI = 0x0E,            //Divide immediately. DIVI OBJ0 TYPE OBJ1 NUMBER
        DIVII = 0x0F,             //Divide inversed immediately. DIVI OBJ0 TYPE NUMBER OBJ1
        SW = 0x10,                //Save Word. SW ARRAY_OBJECT INDEX TYPE:NUMBER
        ADDW = 0x11,              //Add word to ArrayList. ADDW LIST_OBJECT TYPE:DATA                        
        LW = 0x12,                //Load word. LW ARRAY_OBJECT INDEX TARGET_OBJECT
    }
    internal struct SMSSingleCommand
    {
        internal SMSOperation operation;
        internal string OperateDatapath;
        internal string[] parameters;
    }
}
