using ArgumentBuilder;

using System;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = ArgsBuilder.Build<Options>(args);

            Console.WriteLine($"IsAlive: {options.IsAlive}");
            Console.ReadLine();
        }
    }
}
