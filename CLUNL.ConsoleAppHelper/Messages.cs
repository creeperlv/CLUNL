namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Error message, localized.
    /// </summary>
    public struct ErrorMsg
    {
        /// <summary>
        /// ID, will be queried using it.
        /// </summary>
        public string ID;
        /// <summary>
        /// Message fallback string.
        /// </summary>
        public string Fallback;
    }
    /// <summary>
    /// Warning message, localized.
    /// </summary>
    public struct WarnMsg
    {
        /// <summary>
        /// ID, will be queried using it.
        /// </summary>
        public string ID;
        /// <summary>
        /// Message fallback string.
        /// </summary>
        public string Fallback;
    }
}
