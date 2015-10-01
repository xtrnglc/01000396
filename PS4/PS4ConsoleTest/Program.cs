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
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("A1", 10);
            s.SetCellContents("B1", new Formula("A1*2"));
            s.SetCellContents("C1", new Formula("B1+A1"));
            s.SetCellContents("A1", 12.0);

            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[3] { "A1", "B1", "C1" };
            object[] testArray = new object[3] { 12.0, new Formula("A1*2"), new Formula("B1+A1") };
            int i = 0;

            foreach (String t in temp)
            {
                Console.WriteLine(s.GetCellContents(nameArray[i]));
                i++;
            }

        }
    }
}
