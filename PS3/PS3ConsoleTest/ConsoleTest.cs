using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace PS3ConsoleTest
{
    class ConsoleTest
    {
        static void Main(string[] args)
        {
            try
            {
                Formula test = new Formula("5+5");
                Console.WriteLine(test.ToString());
                Console.WriteLine(test.Evaluate(s => 0));
            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong");
            }
            
        }
    }
}
