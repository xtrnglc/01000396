/*
Author: Trung Le
uID: 01000396
Date: 09/17/2015
Course: CS3500
Purpose: Dependency Graph implementation console test
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace DepedencyGraphTest
{
    /// <summary>
    /// Temporary console test to test basic dependency graph methods
    /// </summary>
    class DependecyGraphTest
    {
        static void Main(string[] args)
        {
            DependencyGraph t = new DependencyGraph();

            Console.WriteLine(t.Size);

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");

            t.AddDependency("a", "a");

            Console.WriteLine(t["a"]);

            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            t.ReplaceDependees("d", new HashSet<string>() { "w", "q" });

            foreach (String s in t.GetDependents("a"))
                Console.Write(s + " ");

            foreach (string s in t.GetDependees("d"))
                Console.Write(s + " ");

            Console.WriteLine(t.Size);
        }
    }
}
