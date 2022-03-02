using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLUNL.Utilities
{
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
            int l = cmd.Length;
            char c;
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
