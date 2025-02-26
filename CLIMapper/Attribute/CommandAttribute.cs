using System;

namespace CLIMapper
{
    /// <summary>
    /// Command Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Command.
        /// Use pipe(|) to separate mutiple commands.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// For Stand Alone commands.
        /// When not to expect value from user.
        /// </summary>
        public bool IsStandAlone { get; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initiate the class.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isStandAlone"></param>
        public CommandAttribute(string command, bool isStandAlone = false)
            => (Command, IsStandAlone) = (command, isStandAlone);
        #endregion
    }
}

