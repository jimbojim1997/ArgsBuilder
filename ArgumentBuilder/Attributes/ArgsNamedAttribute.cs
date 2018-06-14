using System;

namespace ArgumentBuilder.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ArgsNamedAttribute : Attribute
    {
        public string Name { get; set; }
        public ArgsValueParseMethod ValueParseMethod { get; set; }
        public ArgsNamedAttribute(string name, ArgsValueParseMethod valueParseMethod)
        {
            Name = name;
            ValueParseMethod = valueParseMethod;
        }
    }
}
