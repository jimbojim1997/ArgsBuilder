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

            Console.WriteLine($"Items:");
            foreach (var i in options.Items) Console.WriteLine(i);
            Console.ReadLine();
        }
    }
}
