using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLUNL.Utilities
{
    /// <summary>
    /// CommandLine class, contains the analyzed results.
    /// </summary>
    public class CommandLine
    {
        /// <summary>
        /// Real Parameters.
        /// </summary>
        public List<Argument> RealParameter = new List<Argument>();
        /// <summary>
        /// Parameters like "-foo:bar;Baz"; "-foo=bar;Baz";
        /// </summary>
        public Dictionary<string, List<string>> SpecifiedParameter = new Dictionary<string, List<string>>();
        /// <summary>
        /// Judge whether a switch with given key is on.(Case Insensitive)
        /// e.g.: "-Foo" means "foo" is on, and the query of "foo" returns true.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool IsOn(string Key)
        {
            foreach (var item in RealParameter)
            {
                if (item.isSwitch)
                {
                    if (item.SwitchName == Key.ToUpper()) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get a List&lt;string&gt; of given key, an empty list if key not found.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public List<string> GetValueList(string Key)
        {
            var K = Key.ToUpper();
            if (SpecifiedParameter.ContainsKey(K))
                return SpecifiedParameter[K];
            else return new List<string>();
        }
        /// <summary>
        /// Get a string array of given key, an empty array if key not found.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string[] GetValueArray(string Key)
        {
            var K = Key.ToUpper();
            if (SpecifiedParameter.ContainsKey(K))
                return SpecifiedParameter[K].ToArray();
            else return new string[0];
        }
    }
    /// <summary>
    /// Real Single Argument
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// Analyzes the command.
        /// </summary>
        /// <param name="singleArgumentString"></param>
        public Argument(string singleArgumentString)
        {
            EntireArgument = singleArgumentString;
            if (EntireArgument.StartsWith("--"))
            {
                isSwitch = true;
                SwitchName = EntireArgument.Substring(2).ToUpper();
            }
            else
            if (EntireArgument[0] == '-' && (EntireArgument.IndexOf(":") > 0 || EntireArgument.IndexOf("=") > 0))
            {

                List<string> vs = new List<string>();
                StringBuilder stringBuilder = new StringBuilder();
                bool a = false;
                bool b = false;
                int l = EntireArgument.Length; char c;
                bool isKeyLocated = false;
                for (int i = 1; i < l; i++)
                {
                    c = EntireArgument[i];
                    if (b == true)
                    {
                        switch (c)
                        {
                            case '\\':
                                stringBuilder.Append('\\');
                                break;
                            case ';':
                                stringBuilder.Append(';');
                                break;
                            case ':':
                                stringBuilder.Append(':');
                                break;
                            case '=':
                                stringBuilder.Append('=');
                                break;
                            default:
                                break;
                        }
                        b = false;
                        continue;
                    }
                    else
                    {
                        if (c == '\\')
                        {
                            b = true;
                            continue;
                        }
                        if (c == ';')
                        {
                            if (a == false)
                            {
                                if (stringBuilder.Length > 0)
                                {
                                    vs.Add((stringBuilder.ToString()));
                                    stringBuilder.Clear();

                                }
                                continue;
                            }
                        }
                        if (isKeyLocated == false)
                        {
                            switch (c)
                            {
                                case ':':
                                case '=':
                                    if (a == false)
                                    {
                                        isKeyLocated = true;

                                        CollectionName = ((stringBuilder.ToString())).ToUpper();
                                        stringBuilder.Clear();
                                        continue;
                                    }

                                    break;
                            }
                        }
                        if (c == '|')
                        {
                            if (a == false)
                            {
                                vs.Add((stringBuilder.ToString()));
                                stringBuilder.Clear();
                                break;
                            }
                        }
                    }
                    stringBuilder.Append(c);
                }
                if (stringBuilder.Length > 0)
                {
                    vs.Add((stringBuilder.ToString()));
                    stringBuilder.Clear();

                }
                if (isKeyLocated == true)
                {
                    isCollection = true;
                    Collection = vs;
                }
            }
        }
        /// <summary>
        /// Indicates if the argument is a switch.
        /// </summary>
        public bool isSwitch = false;
        /// <summary>
        /// The Name of the switch
        /// </summary>
        public string SwitchName = "";
        /// <summary>
        /// Indicated if the argument is a collection.
        /// </summary>
        public bool isCollection = false;
        /// <summary>
        /// The Key of a value collection in the parameter form like -key:value0;value1;...
        /// </summary>
        public string CollectionName = null;
        /// <summary>
        /// The collection of values that in parameter form like: -key:value0;value1...
        /// </summary>
        public List<string> Collection = new List<string>();
        /// <summary>
        /// The original string of the argument;
        /// </summary>
        public string EntireArgument;

        /// <summary>
        /// Using the entire argument;
        /// </summary>
        /// <param name="arg"></param>
        public static implicit operator string(Argument arg)
        {
            return arg.EntireArgument;
        }
    }
    /// <summary>
    /// Tool to help deal with command-line.
    /// </summary>
    public class CommandLineTool
    {
        /// <summary>
        /// Analyze a line of string into CommandLine.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static CommandLine Analyze(string cmd)
        {
            CommandLine commandLine = new CommandLine();
            var parameter = CommandParse(cmd);
            commandLine.RealParameter = parameter;
            foreach (var item in commandLine.RealParameter)
            {
                if (item.isCollection)
                {
                    if (commandLine.SpecifiedParameter.ContainsKey(item.CollectionName))
                    {
                        commandLine.SpecifiedParameter[item.CollectionName].AddRange(item.Collection);
                    }
                    else
                    {
                        commandLine.SpecifiedParameter.Add(item.CollectionName, item.Collection.ToList());
                    }
                }
            }
            return commandLine;
        }
        /// <summary>
        /// Parse a command line into a string list.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static List<Argument> CommandParse(string cmd)
        {
            List<Argument> vs = new List<Argument>();
            StringBuilder stringBuilder = new StringBuilder();
            bool a = false;
            bool b = false;
            int l = cmd.Length; char c;
            for (int i = 0; i < l; i++)
            {
                c = cmd[i];
                if (b == true)
                {
                    switch (c)
                    {
                        case '\"':
                            stringBuilder.Append('\"');
                            break;
                        case '\\':
                            stringBuilder.Append('\\');
                            break;
                        case '\'':
                            stringBuilder.Append('\'');
                            break;
                        case 't':
                            stringBuilder.Append('\t');
                            break;
                        case 'r':
                            stringBuilder.Append('\r');
                            break;
                        case 'n':
                            stringBuilder.Append('\n');
                            break;
                        default:
                            break;
                    }
                    b = false;
                    continue;
                }
                else
                {
                    if (c == '\"')
                    {

                        if (a == false)
                        {
                            a = true;
                            continue;
                        }
                        else
                        {
                            a = false;
                            vs.Add(new Argument(stringBuilder.ToString()));
                            stringBuilder.Clear();
                            continue;
                        }
                    }
                    if (c == ' ')
                    {
                        if (a == false)
                        {
                            if (stringBuilder.Length > 0)
                            {
                                vs.Add(new Argument(stringBuilder.ToString()));
                                stringBuilder.Clear();

                            }
                            continue;
                        }
                    }
                    if (c == '\\')
                    {
                        b = true;
                        continue;
                    }
                    //if (c == '#' || c == ';')
                    //{
                    //    if (a == false)
                    //    {
                    //        vs.Add(new Argument(stringBuilder.ToString()));
                    //        stringBuilder.Clear();
                    //        break;
                    //    }
                    //}
                }
                stringBuilder.Append(c);
            }
            if (stringBuilder.Length > 0)
            {
                vs.Add(new Argument(stringBuilder.ToString()));
                stringBuilder.Clear();

            }
            return vs;
        }
    }
}
