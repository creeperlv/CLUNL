using System;
using System.Collections.Generic;
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
        public List<string> RealParameter = new List<string>();
        /// <summary>
        /// Parameters those starts with '-'
        /// </summary>
        public List<string> PresentSwitches = new List<string>();
        /// <summary>
        /// Judge whether a switch with given key is on.(Case Insensitive)
        /// e.g.: "-Foo" means "foo" is on, and the query of "foo" returns true.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool IsOn(string Key)
        {
            if (PresentSwitches.Contains(Key.ToUpper()))return true;
            return false;
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
            foreach (var item in parameter)
            {
                if (item.StartsWith("-"))
                {
                    commandLine.PresentSwitches.Add(item.Substring(1).ToUpper());
                }
            }
            return commandLine;
        }
        /// <summary>
        /// Parse a command line into a string list.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static List<string> CommandParse(string cmd)
        {
            List<string> vs = new List<string>();
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
                            vs.Add(stringBuilder.ToString() );
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

                                vs.Add(stringBuilder.ToString());
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
                    if (c == '#' || c == ';')
                    {
                        if (a == false)
                        {
                            vs.Add(stringBuilder.ToString());
                            stringBuilder.Clear();
                            break;
                        }
                    }
                }
                stringBuilder.Append(c);
            }
            if (stringBuilder.Length > 0)
            {
                vs.Add(stringBuilder.ToString());
                stringBuilder.Clear();

            }
            return vs;
        }
    }
}
