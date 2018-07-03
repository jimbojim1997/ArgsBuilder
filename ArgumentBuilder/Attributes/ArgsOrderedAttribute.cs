using System;

namespace ArgumentBuilder.Attributes
{
    /// <summary>
    /// Used to identify an ordered argument
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgsOrderedAttribute : Attribute
    {
        /// <summary>
        /// The index of the argument in the <c>args[]</c>. Ignores <see cref="ArgsNamedAttribute"/> arguments.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Used to identify an ordered argument
        /// </summary>
        /// <param name="index">The index of the argument in the <c>args[]</c>. Ignores <see cref="ArgsNamedAttribute"/> arguments.</param>
        public ArgsOrderedAttribute(int index)
        {
            Index = index;
        }
    }
}
