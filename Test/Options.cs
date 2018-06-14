using ArgumentBuilder.Attributes;

using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    class Options
    {
        [ArgsOrdered(0)]
        public string InPath { get; set; }

        [ArgsOrdered(1)]
        public string OutPath { get; set; }

        [ArgsNamed("--other-option", ArgsValueParseMethod.Space)]
        [ArgsNamed("-o", ArgsValueParseMethod.Space)]
        public string OtherOption { get; set; }

        public Options() { }
    }
}
