using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace EvaluatorTester
{
    /// <summary>
    /// Console Application to test the Formula Evaluator class
    /// </summary>
    class EvaluatorTester
    {
        /// <summary>
        /// Evaluates variables
        /// Returns 1 whenever undefined variable is encountered
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int variableEvaluator(string s)
        {
            if (s.Equals("X"))
            {
                return 5;
            }

            else
            {
                return 2;
            }
        }
        /// <summary>
        /// Main Test Driver
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("This program tests the infix calculator class library");
            Console.WriteLine("It accepts non negative integers and pre defined variables and the following operators ( ) + - * /");
            Console.WriteLine("Does not accept white space");
            Console.WriteLine("Example: (2+3)-10/5+4*6 should evaluate to 27");

            try
            {
                double numTest = Evaluator.Evaluate("30/(X+S)/2+(1+2*3)", variableEvaluator);
                Console.WriteLine("\nThe expression evaluates to: " + numTest);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
