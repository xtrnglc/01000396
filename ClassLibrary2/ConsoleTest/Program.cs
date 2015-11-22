using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemNameSpace;
using Entities;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystem f = new FileSystem();
            f.Create("drive", "cdrive", "");
            f.Create("folder", "folder1", "cdrive");
            f.Create("folder", "folder2", "cdrive\\folder1");

            f.Create("text", "duck", "cdrive\\folder1");
            f.WriteToFile("cdrive\\folder1\\duck", "HORSES ARE COOl");

            f.Create("text", "duck2", "cdrive\\folder1");
            f.WriteToFile("cdrive\\folder1\\duck2", "YES1");;
            
            f.Create("text", "duck3", "cdrive");
            f.WriteToFile("cdrive\\duck3", "1234567890");

            f.Create("text", "duck3", "cdrive\\folder1\\folder2");
            f.WriteToFile("cdrive\\folder1\\folder2\\duck3", "HORSES ARE COOl123");



            //Folder2 = 18
            //Folder1 = 36
            //cdrive = 36
            Console.Write("folder2: ");
            f.printSize("cdrive\\folder1\\folder2");
            Console.Write("folder1: ");
            f.printSize("cdrive\\folder1");
            Console.Write("cdrive ");
            f.printSize("cdrive");
            f.Move("cdrive\\folder1\\folder2\\duck3", "cdrive");
            Console.WriteLine("after move \n");
            Console.Write("folder2: ");
            f.printSize("cdrive\\folder1\\folder2");
            Console.Write("folder1: ");
            f.printSize("cdrive\\folder1");
            Console.Write("cdrive: ");
            f.printSize("cdrive");
            Console.ReadLine();
        }
    }
}
