﻿using System;
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
            
            t.AddDependency("b", "e");
            t.AddDependency("e", "f");
            t.AddDependency("c", "d");
            t.RemoveDependency("e", "f");

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });

            foreach (String s in t.GetDependents("a"))
                Console.Write(s + " ");

            foreach (string s in t.GetDependees("d"))
                Console.Write(s + " ");

            Console.WriteLine(t.Size);

        }
    }
}
