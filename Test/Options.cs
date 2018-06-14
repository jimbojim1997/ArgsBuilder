using ArgumentBuilder.Attributes;

using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    class Options
    {
        [ArgsNamed("--valueA", ArgsValueParseMethod.Equals)]
        public string ValueA { get; set; }

        [ArgsNamed("--valueB", ArgsValueParseMethod.Equals)]
        public string ValueB { get; set; }

        public Options() { }
    }
}
