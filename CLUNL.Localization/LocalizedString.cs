using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CLUNL.Localization
{
    /// <summary>
    /// Localized via Language.
    /// </summary>
    public class LocalizedString
    {
        /// <summary>
        /// Initialize the string with LanguageID and fallback.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Fallback"></param>
        public LocalizedString(string ID,string Fallback)
        {
            this.ID = ID;
            this.Fallback = Fallback;
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
            return Language.Find(ID, Fallback);
        }
        /// <summary>
        /// Return the value from ToString();
        /// </summary>
        /// <param name="L"></param>
        public static implicit operator string(LocalizedString L)
        {
            return L.ToString();
        }
    }
}
