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
            var list = new List<String>();
            var rand = new Random();

            for (int i = 0; i < 40; i++)
            {
                list.Add(String.Format("ABC{0:000}", rand.Next(1, 999)));
            }

            var comparer = new StringLogicalComparer();
            list.Sort(comparer);
        }
    }
}
