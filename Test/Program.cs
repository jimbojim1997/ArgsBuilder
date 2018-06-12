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

            Console.WriteLine($"Person {options.Name} is aged {options.Age} and is {(options.IsAlive ? "alive" : "dead")} and likes {String.Join(", ", options.Likes.ToArray())}.");
            Console.ReadLine();
        }
    }
}
