using System;

namespace ArgumentBuilder.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ArgsNamedAttribute : Attribute
    {
        public string Name { get; set; }
        public ArgsValueParseMethod ValueParseMethod { get; set; }
        public ArgsNamedAttribute(string name, ArgsValueParseMethod valueParseMethod = ArgsValueParseMethod.Boolean)
        {
            Name = name;
            ValueParseMethod = valueParseMethod;
        }
    }
}
