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
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "c");
            t.AddDependency("e", "d");

        }
    }
}
