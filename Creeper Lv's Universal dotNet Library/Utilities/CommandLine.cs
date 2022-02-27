using System.Collections.Generic;

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
                    if (item.SwitchName == Key.ToUpper())
                        return true;
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
            else
                return new List<string>();
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
            else
                return new string[0];
        }
    }
}
