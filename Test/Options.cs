using System;
using ArgumentBuilder.Attributes;
using System.Collections.Generic;

namespace Test
{
    class Options
    {
        [ArgsNamed("--name", ArgsValueParseMethod.Space)]
        public String Name { get; set; }

        [ArgsNamed("--age", ArgsValueParseMethod.Space)]
        public int Age { get; set; }

        [ArgsNamed("--alive", ArgsValueParseMethod.Boolean)]
        public bool IsAlive { get; set; }

        [ArgsNamed("--hobby", ArgsValueParseMethod.Space)]
        [ArgsNamed("-h", ArgsValueParseMethod.Space)]
        public IEnumerable<string> Hobbies { get; set; }

        public Options() { }
    }
}
