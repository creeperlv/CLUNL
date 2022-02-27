using System.Collections.Generic;

namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Parameter List.
    /// </summary>
    public class ParameterList
    {
        /// <summary>
        /// Options.
        /// </summary>
        public Dictionary<string, object> Options = new Dictionary<string, object>();
        /// <summary>
        /// Parameters and its variants, e.g.: -Output and --o.
        /// </summary>
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();//Variant -> main key
        internal void AddKey(string Key, object Value)
        {
            if (!Options.ContainsKey(Parameters[Key.ToUpper()]))
                Options.Add(Parameters[Key.ToUpper()], Value);
            else
                Options[Parameters[Key.ToUpper()]] = Value;
        }
        /// <summary>
        /// Try query a parameter.
        /// </summary>
        /// <param name="KeyVariant"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryQuery(string KeyVariant, ref object result)
        {
            result = Query(KeyVariant);
            return result == null;
        }
        /// <summary>
        /// Try query a parameter.
        /// </summary>
        /// <param name="KeyVariant"></param>
        /// <param name="TResult"></param>
        /// <returns></returns>
        public bool TryQuery<T>(string KeyVariant, ref T TResult)
        {

            var result = Query(KeyVariant);
            var isHit = result == null;

            if (isHit)
                TResult = (T)result;

            return isHit;
        }
        /// <summary>
        /// Query a parameter. Default value will is null.
        /// </summary>
        /// <param name="KeyVariant"></param>
        /// <returns></returns>
        public object Query(string KeyVariant)
        {
            return Options[Parameters[KeyVariant.ToUpper()]];
        }
        /// <summary>
        /// Query a parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyVariant"></param>
        /// <returns></returns>
        public T Query<T>(string KeyVariant)
        {
            return (T)Options[Parameters[KeyVariant.ToUpper()]];
        }
        /// <summary>
        /// Internal usage.
        /// </summary>
        /// <param name="dependentFeatureAttribute"></param>
        public void ApplyDescription(DependentFeatureAttribute dependentFeatureAttribute)
        {
            foreach (var item in dependentFeatureAttribute.Options)
            {
                var variants = item.Split(',');
                foreach (var variant in variants)
                {
                    Parameters.Add(variant.ToUpper(), variants[0].ToUpper());
                }
                Options.Add(variants[0].ToUpper(), null);
            }
        }
    }
}
