using ArgumentBuilder.Attributes;

using System.Collections.Generic;
using System.ComponentModel;

namespace Test
{
    class Options
    {
        [ArgsNamed("--is-dead", ArgsValueParseMethod.BooleanInverted)]
        [DefaultValue(true)]
        public bool IsAlive { get; set; }

        public Options() { }
    }
}
