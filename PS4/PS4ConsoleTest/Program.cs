using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace PS4ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("A1", 10);
            double x = (double)s.GetCellContents("A1");
        }
    }
}
