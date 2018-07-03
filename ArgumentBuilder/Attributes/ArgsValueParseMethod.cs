namespace ArgumentBuilder.Attributes
{
    /// <summary>
    /// The key-value-pair parse method on <see cref="ArgsNamedAttribute"/> arguments.
    /// </summary>
    public enum ArgsValueParseMethod
    {
        /// <summary>
        /// If the argument exists the value is set to True.
        /// </summary>
        Boolean,

        /// <summary>
        /// If the argument exists the value is set to False. Used with <see cref="System.DefaultValueAttribute"/> to set the value if not set.
        /// </summary>
        BooleanInverted,

        /// <summary>
        /// The argument key and value are separated by a space.
        /// </summary>
        /// <example>--path C:\temp\test.txt</example>
        /// /// <example>--path "C:\temp\test.txt"</example>
        Space,

        /// <summary>
        /// The argument key and value are separated by an <c>=</c> (equals) symbol.
        /// </summary>
        /// <example>--path=C:\temp\test.txt</example>
        /// <example>"--path=C:\temp\test.txt"</example>
        Equals,

        /// <summary>
        /// The argument key and value are separated by a <c>:</c> colon symbol.
        /// </summary>
        /// <example>--path:C:\temp\test.txt</example>
        /// <example>"--path:C:\temp\test.txt"</example>
        Colon
    }
}
