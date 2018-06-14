using ArgumentBuilder.Attributes;

using System.Collections.Generic;

namespace Test
{
    class Options
    {
        [ArgsNamed("--name", ArgsValueParseMethod.Space)]
        public string Name { get; set; }

        [ArgsNamed("--age", ArgsValueParseMethod.Space)]
        public int Age { get; set; }

        [ArgsNamed("--is-alive", ArgsValueParseMethod.Boolean)]
        public bool IsAlive { get; set; }

        [ArgsNamed("--likes", ArgsValueParseMethod.Space)]
        public IEnumerable<string> Likes { get; set; }

        public Options() { }
    }
}
