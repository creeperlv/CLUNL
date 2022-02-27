using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CLUNL.Localization
{
    /// <summary>
    /// Language definition.
    /// </summary>
    [Serializable]
    public class LanguageDefinition : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Gets or sets the string associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return LanguageStrings[key];
            }
            set
            {
                LanguageStrings[key] = value;
            }
        }
        /// <summary>
        /// Real data.
        /// </summary>
        public Dictionary<string, string> LanguageStrings = new Dictionary<string, string>();
        /// <summary>
        /// Determines if the language definition contains target key.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool ContainsKey(string Key)
        {
            return LanguageStrings.ContainsKey(Key);
        }
        /// <summary>
        /// Write the value to target key.
        /// </summary>
        /// <param name="K"></param>
        /// <param name="V"></param>
        public void Write(string K, string V)
        {
            if (LanguageStrings.ContainsKey(K))
            {
                LanguageStrings[K] = V;
            }
            else
            {
                LanguageStrings.Add(K, V);
            }
        }
        /// <summary>
        /// Add a string.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Add(string Key, string Value)
        {
            LanguageStrings.Add(Key, Value);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the System.Collections.Generic.Dictionary&lt;string,string&gt;.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in LanguageStrings)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return LanguageStrings.GetEnumerator();
        }
        /// <summary>
        /// Serialize definition into a stirng list.
        /// </summary>
        /// <returns></returns>
        public List<string> Serialize()
        {
            List<string> RESULT = new List<string>();
            RESULT.Add(Language.GeneratorPrefix);
            foreach (var item in LanguageStrings)
            {
                RESULT.Add($"{item.Key}={item.Value.Replace("\r", "\\r").Replace("\n", "\\n")}");
            }
            return RESULT;
        }
        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="file"></param>
        public void SaveToFile(FileInfo file)
        {
            using (var stream = file.Open(FileMode.OpenOrCreate))
            {

                using (var SW = new StreamWriter(stream))
                {

                    foreach (var item in new EnumerativeSerializer(this))
                    {
                        SW.WriteLine(item);
                    }
                }
            }
        }
        /// <summary>
        /// Load form string array.
        /// </summary>
        /// <param name="contents"></param>
        public void LoadFromStringArray(string[] contents)
        {
            foreach (var item in contents)
            {
                var pitem = item.Trim();
                if (pitem.StartsWith("#") || pitem.StartsWith(";") || pitem.StartsWith("//"))
                {
                    continue;
                }
                else
                {
                    var index = pitem.IndexOf(Language.EqualSymbol);
                    if (index > 0)
                    {
                        var pairName = pitem.Substring(0, index);
                        var pairContent = pitem.Substring(index + 1);
                        if (!LanguageStrings.ContainsKey(pairName))
                            LanguageStrings.Add(pairName, pairContent);
                        else LanguageStrings[pairName] = pairContent;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Enumerative Language Definition Serializer
    /// </summary>
    public class EnumerativeSerializer : IEnumerable<string>
    {
        LanguageDefinition def;
        /// <summary>
        /// Initialize serializer with data.
        /// </summary>
        /// <param name="def"></param>
        public EnumerativeSerializer(LanguageDefinition def)
        {
            this.def = def;
        }
        /// <summary>
        /// Returns an enumerator that iterates through the serialized content by line.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            yield return (Language.GeneratorPrefix);
            foreach (var item in def)
            {
                yield return ($"{item.Key}={item.Value.Replace("\r", "\\r").Replace("\n", "\\n")}");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return (Language.GeneratorPrefix);
            foreach (var item in def)
            {
                yield return ($"{item.Key}={item.Value.Replace("\r", "\\r").Replace("\n", "\\n")}");
            }
        }
    }
}
