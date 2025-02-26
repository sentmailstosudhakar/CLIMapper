using System;

namespace CLIMapper
{
    /// <summary>
    /// Mapper Exception.
    /// </summary>
    public sealed class MapperException : Exception
    {
        /// <summary>
        /// Mapper Exception.
        /// </summary>
        public MapperException() : base() { }

        /// <summary>
        /// Mapper Exception.
        /// </summary>
        public MapperException(string message) : base(message) { }

        /// <summary>
        /// Mapper Exception.
        /// </summary>
        public MapperException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Trace removed.
        /// </summary>
        public override string StackTrace => null;
    }
}