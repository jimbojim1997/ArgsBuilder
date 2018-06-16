using ArgumentBuilder.Attributes;

using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    class Options
    {
        [ArgsNamed("-i", ArgsValueParseMethod.Space)]
        public IEnumerable<double> Items { get; set; }

        public Options() { }
    }
}
