using System.Collections;
using System.Collections.Generic;

namespace CLUNL.Localization
{
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
