/*
Author: Trung Le
Date: 09/28/2015
PS4 Console Test
Used mainly for debugging
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using SS;

namespace PS4ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("B1", "=A1+10");
            s.SetContentsOfCell("C1", ("=5+10"));
            s.SetContentsOfCell("D1", "=B1+4");
            

            s.SetContentsOfCell("A1", "12");

            Console.WriteLine(s.GetCellValue("A1"));
            Console.WriteLine(s.GetCellValue("B1"));
            Console.WriteLine(s.GetCellValue("C1"));
            Console.WriteLine(s.GetCellValue("D1"));
            Console.WriteLine();

            Console.WriteLine(s.GetCellContents("A1"));
            Console.WriteLine(s.GetCellContents("B1"));
            Console.WriteLine(s.GetCellContents("C1"));
            Console.WriteLine(s.GetCellContents("D1"));


        }
    }
}
