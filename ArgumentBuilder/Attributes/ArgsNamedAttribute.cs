using System;

namespace ArgumentBuilder.Attributes
{
    /// <summary>
    /// Used to identify a named argument.
    /// </summary>
    /// <example>--path</example>
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ArgsNamedAttribute : Attribute
    {
        /// <summary>
        /// The name of the argument.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parse method of the argument.
        /// </summary>
        public ArgsValueParseMethod ValueParseMethod { get; set; }

        /// <summary>
        /// Used to identify a named argument.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="valueParseMethod">The parse method of the argument.</param>
        public ArgsNamedAttribute(string name, ArgsValueParseMethod valueParseMethod)
        {
            Name = name;
            ValueParseMethod = valueParseMethod;
        }
    }
}
