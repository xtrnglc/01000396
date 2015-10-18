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

            for (int i = 0; i < 500; i++)
            {
                s.SetCellContents("A1" + i, new Formula("A1" + (i + 1)));
            }
            HashSet<string> firstCells = new HashSet<string>();
            HashSet<string> lastCells = new HashSet<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.Add("A1" + i);
                lastCells.Add("A1" + (i + 250));
            }


            s.SetCellContents("A1249", 25.0);
            s.SetCellContents("A1499", 0);

            Console.Write("DFDSA");
        }
    }
}
