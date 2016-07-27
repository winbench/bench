namespace Mastersign.Bench
{
    /// <summary>
    /// The enumeration of possible status icon types of an app.
    /// </summary>
    public enum AppStatusIcon
    {
        /// <summary>
        /// No status icon. The app is not activated, not installed, and not cached.
        /// </summary>
        None,

        /// <summary>
        /// The app is activated and the status of the app is OK.
        /// </summary>
        OK,

        /// <summary>
        /// There is an info message, regarding this app.
        /// </summary>
        Info,

        /// <summary>
        /// The app is not activated, but is is cached.
        /// </summary>
        Cached,

        /// <summary>
        /// The app is not activated, but installed.
        /// </summary>
        Tolerated,

        /// <summary>
        /// The app is explicitly deactivated.
        /// </summary>
        Blocked,

        /// <summary>
        /// There is a pending task, regarding this app.
        /// </summary>
        Task,

        /// <summary>
        /// There is an error message, regarding this app.
        /// </summary>
        Warning
    }
}
