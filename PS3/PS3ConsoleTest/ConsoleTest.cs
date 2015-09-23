/*
Author: Trung Le
Course: CS 3500
Date: 09/21/2015
Purpose: Formula Class Console Test. Mainly used for debugging.
*/
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
                Formula test = new Formula("5+1xx");
                Console.WriteLine(test.Evaluate(s => 0));

            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong");
            }
            
        }
    }
}
