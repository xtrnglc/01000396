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
                Formula test3 = null;
                Formula test4 = null;

                Console.WriteLine(test3 == test4);

                Formula test = new Formula("x1 + x2 - y3 * y4");
                IEnumerable<string> temp = test.GetVariables();
                foreach(string s in temp)
                {
                    Console.WriteLine(s);
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong");
            }
            
        }
    }
}
