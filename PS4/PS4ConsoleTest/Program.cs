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
using System.Text.RegularExpressions;

namespace PS4ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractSpreadsheet s = new Spreadsheet(validator2, normalizer2, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=ee5+2");
            s.SetContentsOfCell("D1", "Hello World");
        }

        /// <summary>
        /// Normalizer test, returns X
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string normalizer2(string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// return true always
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool validator1(string s)
        {
            return true;
        }

        /// <summary>
        /// Returns true only if variable is form AA5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool validator2(String s)
        {
            return Regex.IsMatch(s, "^([A-Z]){2}[5]$");
        }
    }
}
