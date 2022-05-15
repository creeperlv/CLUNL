using System;
using System.Runtime.CompilerServices;

namespace CLUNL.Localization
{
    /// <summary>
    /// Localized via Language.
    /// </summary>
    [Serializable]
    public class LocalizedString
    {
        /// <summary>
        /// Used by string.Format().
        /// </summary>
        public object[] arguments;
        /// <summary>
        /// Empty constructor for serialization purpose
        /// </summary>
        public LocalizedString()
        {
            arguments = new object[0];
            ID = "";
            Fallback = "";
        }
        /// <summary>
        /// Initialize the string with LanguageID and fallback.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Fallback"></param>
        /// <param name="arguments"></param>
        public LocalizedString(string ID, string Fallback, params object[] arguments)
        {
            this.ID = ID;
            this.Fallback = Fallback;
            this.arguments = arguments;
        }
        /// <summary>
        /// ID in language definition.
        /// </summary>
        public string ID="";
        /// <summary>
        /// Fallback value once it is not found in language definition.
        /// </summary>
        public string Fallback="";
        /// <summary>
        /// Return the localized string.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            if (arguments != null)
                return string.Format(Language.Find(ID, Fallback), arguments);
            else return Language.Find(ID, Fallback);
        }
        /// <summary>
        /// Return the value from ToString();
        /// </summary>
        /// <param name="L"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(LocalizedString L)
        {
            return L.ToString();
        }
    }
}
