using System;
using ArgumentBuilder.Attributes;
using System.Collections.Generic;

namespace Test
{
    class Options
    {
        [ArgsNamed("--name", ArgsValueParseMethod.Space)]
        public List<String> Name { get; set; }

        public Options() { }
    }
}
