using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace DepedencyGraphTest
{
    class DependecyGraphTest
    {
        static void Main(string[] args)
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Console.WriteLine(t.HasDependees("a"));
            Console.WriteLine(t.HasDependees("b"));
            Console.WriteLine(t.HasDependents("a"));
            Console.WriteLine(t.HasDependents("b"));
        }
    }
}
