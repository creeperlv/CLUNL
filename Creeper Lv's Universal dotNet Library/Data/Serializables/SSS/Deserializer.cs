using CLUNL.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// Shell Scrip Style
/// </summary>
namespace CLUNL.Data.Serializables.SSS
{
    /// <summary>
    /// Deserializer
    /// </summary>
    public class Deserializer
    {
        /// <summary>
        /// Deserialize a list content for you have no known base class.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<object> Deserialize(List<string> content)
        {
            return Deserialize<object>(content);
        }
        /// <summary>
        /// Deserialize a list content for you have no known base class enumeratively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Reader"></param>
        /// <param name="result"></param>
        public static void Deserialize<T>(TextReader Reader, ref List<T> result)
        {
            object processing = null;
            bool isInited = false;
            Type t = null;
            string line;
            while ((line = Reader.ReadLine()) != null)
            {
                var item = line.Trim();
                if (item.StartsWith("#"))
                {
                    //Macro preserve.
                    continue;
                }
                if (item.StartsWith(";") || item.StartsWith("\\\\") || item.StartsWith("$"))
                {

                    continue;
                }
                var cmd = CommandLineTool.Analyze(item);
                if (isInited)
                {
                    if (cmd.RealParameter[0].EntireArgument == "+" || cmd.RealParameter[0].EntireArgument == "\\")
                    {

                    }
                    else
                    {
                        result.Add((T)processing);
                        foreach (var DLL in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            t = DLL.GetType(cmd.RealParameter[0].EntireArgument);
                            if (t != null)
                            {
                                processing = (T)Activator.CreateInstance(t);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (cmd.RealParameter[0].EntireArgument == "+" || cmd.RealParameter[0].EntireArgument == "\\")
                    {

                    }
                    else
                    {
                        foreach (var DLL in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            t = DLL.GetType(cmd.RealParameter[0].EntireArgument);
                            if (t != null)
                            {
                                processing = (T)Activator.CreateInstance(t);
                                isInited = true;
                                break;
                            }
                        }

                    }
                }
                for (int A = 1; A < cmd.RealParameter.Count; A++)
                {
                    var arg = cmd.RealParameter[A];
                    if (arg.isCollection)
                    {
                        var Field = t.GetField(arg.CollectionNameStrictCase);
                        var FT = Field.FieldType;
                        object value = Convert.ChangeType(arg.UnsegmentedCollectionString, FT);

                        Field.SetValue(processing, value);
                    }
                }
            }
        }
        /// <summary>
        /// Deserialize a list content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<T> Deserialize<T>(List<string> content)
        {
            List<T> result = new List<T>();
            object processing = null;
            bool isInited = false;
            Type t = null;
            for (int i = 0; i < content.Count; i++)
            {
                var item = content[i].Trim();
                if (item.StartsWith("#"))
                {
                    //Macro preserve.
                    continue;
                }
                if (item.StartsWith(";") || item.StartsWith("\\\\") || item.StartsWith("$"))
                {

                    continue;
                }
                var cmd = CommandLineTool.Analyze(item);
                if (isInited)
                {
                    if (cmd.RealParameter[0].EntireArgument == "+" || cmd.RealParameter[0].EntireArgument == "\\")
                    {

                    }
                    else
                    {
                        result.Add((T)processing);
                        foreach (var DLL in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            t = DLL.GetType(cmd.RealParameter[0].EntireArgument);
                            if (t != null)
                            {
                                processing = (T)Activator.CreateInstance(t);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (cmd.RealParameter[0].EntireArgument == "+" || cmd.RealParameter[0].EntireArgument == "\\")
                    {

                    }
                    else
                    {
                        foreach (var DLL in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            t = DLL.GetType(cmd.RealParameter[0].EntireArgument);
                            if (t != null)
                            {
                                processing = (T)Activator.CreateInstance(t);
                                isInited = true;
                                break;
                            }
                        }

                    }
                }
                for (int A = 1; A < cmd.RealParameter.Count; A++)
                {
                    var arg = cmd.RealParameter[A];
                    if (arg.isCollection)
                    {
                        var Field = t.GetField(arg.CollectionNameStrictCase);
                        var FT = Field.FieldType;
                        object value = Convert.ChangeType(arg.UnsegmentedCollectionString, FT);

                        Field.SetValue(processing, value);
                    }
                }
            }
            return result;
        }
    }
}
