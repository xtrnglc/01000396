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
using System.Text.RegularExpressions;

namespace PS3ConsoleTest
{
    class ConsoleTest
    {
        static void Main(string[] args)
        {
            try
            {
                Formula test = new Formula("8+yy2+z2+we2+qw2", normalizer2, validator2);

                IEnumerable<string> temp = test.GetVariables();

                foreach (String s in temp)
                {
                    Console.Write(s+" ");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong");
            } 
        }

        /// <summary>
        /// Normalizer test, returns X
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string normalizer1(string s)
        {
            if(s == "y2")
            {
                return "x5";
            }

            else if(s == "z2")
            {
                return "y5";
            }

            else if(s == "w2")
            {
                return "z5";
            }

            else
            {
                return "w5";
            }
        }

        /// <summary>
        /// Normalizer test, returns X
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string normalizer2(string s)
        {
            return s.ToUpper();
        }

        public static bool validator(string s)
        {
            return true;
        }

        public static bool validator2(String s)
        {
            return Regex.IsMatch(s, "^([A-Z]){2}[5]$");
        }
    }


}



/*
/// <summary>
        /// Test for normalizer
        ///</summary>
        [TestMethod()]
        public void ConstructorTest23()
        {
            Formula test = new Formula("8+y2+z2+w2+q2", normalizer1, validator);

            IEnumerable<string> temp = test.GetVariables();
            string[] testArray = new string[4] { "X", "X", "X", "X" };
            int i = 0;

            foreach (String s in temp)
            {
                Assert.AreEqual(s, testArray[i]);
                i++;
            }
        }

        

        
*/
