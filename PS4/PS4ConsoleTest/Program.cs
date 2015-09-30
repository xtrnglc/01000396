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
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", 10);
            s.SetCellContents("C1", "hello");
            s.SetCellContents("D1", "");

            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();

        }
    }
}
