using System.Collections.Generic;
using System.Text;

namespace CLUNL.Scripting
{
    public static class Utilities
    {
        public static List<string> ResolveParameters(string line)
        {
            List<string> vs = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            bool a = false;
            bool b = false;
            int l = line.Length; char c;
            for (int i = 0; i < l; i++)
            {
                c = line[i];
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
                            vs.Add(stringBuilder.ToString());
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
