using System;

namespace ArgumentBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgsOrderedAttribute : Attribute
    {
        public int Index { get; set; }
        public ArgsOrderedAttribute(int index)
        {
            Index = index;
        }
    }
}
