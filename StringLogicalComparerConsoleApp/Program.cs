using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringLogicalComparerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new[] { "AB1", "AB10", "AB21", "AB3", "AB11", "AB2", "AB20", "AB30", "AB31" };
            Console.WriteLine("SOURCE: {0}", String.Join(", ", array));

            var ordinalComparer = StringComparer.Ordinal;
            Array.Sort(array, ordinalComparer);

            Console.WriteLine("ORGINAL: {0}", String.Join(", ", array));

            var logicalComparer = StringLogicalComparer.Ordinal;
            Array.Sort(array, logicalComparer);

            Console.WriteLine("LOGICAL: {0}", String.Join(", ", array));
        }
    }
}
