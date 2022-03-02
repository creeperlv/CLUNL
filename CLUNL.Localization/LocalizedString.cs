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
        object[] arguments;
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
        private string ID;
        private string Fallback;
        /// <summary>
        /// Return the localized string.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            if (arguments != null)
                return String.Format(Language.Find(ID, Fallback), arguments);
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
