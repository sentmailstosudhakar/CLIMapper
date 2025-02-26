using System;

namespace CLIMapper
{

    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        public static void Log(string message, Severity severity = Severity.Info) => Console.WriteLine($"{severity.ToString().ToUpper(MapperConstant.cultureInfo)}: {message}");

        /// <summary>
        /// Log Severity.
        /// </summary>
        public enum Severity
        {
            /// <summary>
            /// Information.
            /// </summary>
            Info = 0,

            /// <summary>
            /// Debug.
            /// </summary>
            Debug,

            /// <summary>
            /// Warning.
            /// </summary>
            Warning,

            /// <summary>
            /// Error.
            /// </summary>
            Error
        }
    }
}