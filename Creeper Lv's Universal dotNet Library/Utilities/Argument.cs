using System.Collections.Generic;
using System.Text;

namespace CLUNL.Utilities
{
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
                StringBuilder stringBuilder2 = new StringBuilder();
                bool a = false;
                bool b = false;
                int l = EntireArgument.Length;
                char c;
                bool isKeyLocated = false;
                for (int i = 1; i < l; i++)
                {
                    c = EntireArgument[i];
                    if (isKeyLocated)
                        stringBuilder2.Append(c);
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

                                        CollectionNameStrictCase = stringBuilder.ToString();
                                        CollectionName = CollectionNameStrictCase.ToUpper();
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
                    UnsegmentedCollectionString = stringBuilder2.ToString();
                    isCollection = true;
                    Collection = vs;
                }
                stringBuilder2.Clear();
                stringBuilder.Clear();
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
        /// The Key of a value collection in the parameter form like `-key:value0;value1;...`, upper cased.
        /// </summary>
        public string CollectionName = null;
        /// <summary>
        /// The Key of a value collection in the parameter form like `-key:value0;value1;...`, the case is strict to original string
        /// </summary>
        public string CollectionNameStrictCase = null;
        /// <summary>
        /// The collection of values that in parameter form like: -key:value0;value1...
        /// </summary>
        public List<string> Collection = new List<string>();
        /// <summary>
        /// The original string of the argument;
        /// </summary>
        public string EntireArgument;
        /// <summary>
        /// The unsegmented string of collection part;
        /// </summary>
        public string UnsegmentedCollectionString;
        /// <summary>
        /// Using the entire argument;
        /// </summary>
        /// <param name="arg"></param>
        public static implicit operator string(Argument arg)
        {
            return arg.EntireArgument;
        }
    }
}
