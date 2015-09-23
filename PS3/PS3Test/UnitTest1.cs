﻿/*
Author: Trung Le
Course: CS 3500
Date: 09/21/2015
Purpose: Formula Class Unit Testing. Covers 90% of code. All relevant code covered.
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace PS3Test
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Test evaluate method using two integers
        /// </summary>
        [TestMethod]
        public void EvaluatorTest1()
        {
            Formula test = new Formula("10+5");
            Assert.AreEqual(test.Evaluate(s => 0), 15);

            Formula test2 = new Formula("10-5");
            Assert.AreEqual(test2.Evaluate(s => 0), 5);

            Formula test3 = new Formula("10/5");
            Assert.AreEqual(test3.Evaluate(s => 0), 2);

            Formula test4 = new Formula("10*5");
            Assert.AreEqual(test4.Evaluate(s => 0), 50);
        }

        /// <summary>
        /// Test evaluate method using two doubles
        /// </summary>
        [TestMethod]
        public void EvaluatorTest2()
        {
            Formula test = new Formula("5.0+10.0");
            Assert.AreEqual(test.Evaluate(s => 0), 15);

            Formula test2 = new Formula("10.000-5.0");
            Assert.AreEqual(test2.Evaluate(s => 0), 5);

            Formula test3 = new Formula("10.0/5.00");
            Assert.AreEqual(test3.Evaluate(s => 0), 2);

            Formula test4 = new Formula("10.00*5.00");
            Assert.AreEqual(test4.Evaluate(s => 0), 50);
        }

        /// <summary>
        /// Test for unequal parentheses, more closing
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest1()
        {
            Formula test = new Formula("((4+1)))");
        }

        /// <summary>
        /// Test for unequal parentheses, more opening
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest2()
        {
            Formula test = new Formula("(((4+1))");
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest3()
        {
            Formula test = new Formula("8(");
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest4()
        {
            Formula test = new Formula("(8");
        }

        /// <summary>
        /// Test .Equals
        /// </summary>
        [TestMethod()]
        public void EqualsTest1()
        {
            Formula test = new Formula("10+5");
            Assert.AreEqual(false, test.Equals(null));

            Assert.AreEqual(false, test.Equals(6));

            Assert.AreEqual(true, new Formula("x1+y2").Equals(new Formula("x1  +  y2")));

            Assert.AreEqual(false, new Formula("x1+y2").Equals(new Formula("y2+x1")));

            Assert.AreEqual(true, new Formula("5.0 + x5").Equals(new Formula("5.000 + x5")));

            Assert.AreEqual(false, new Formula("5.0 + x5").Equals(new Formula("5.00 + X5")));
        }

        /// <summary>
        /// Test ==
        /// </summary>
        [TestMethod()]
        public void EqualsTest2()
        {
            Formula test = new Formula("10+5");
            Formula test2 = new Formula("2+2");

            Assert.AreEqual(false, test == null);

            Assert.AreEqual(false, test == test2);

            Assert.AreEqual(true, new Formula("x1+y2") == (new Formula("x1  +  y2")));

            Assert.AreEqual(false, new Formula("x1+y2") == (new Formula("y2+x1")));

            Assert.AreEqual(true, new Formula("5.0 + x5") == (new Formula("5.000 + x5")));

            Assert.AreEqual(false, new Formula("5.0 + x5") == (new Formula("5.00 + X5")));
        }

        /// <summary>
        /// Test !=
        /// </summary>
        [TestMethod()]
        public void NotEqualsTest()
        {
            Formula test = new Formula("10+5");
            Formula test2 = new Formula("2+2");

            Assert.AreEqual(true, test != null);

            Assert.AreEqual(true, test != test2);

            Assert.AreEqual(false, new Formula("x1+y2") != (new Formula("x1  +  y2")));

            Assert.AreEqual(true, new Formula("x1+y2") != (new Formula("y2+x1")));

            Assert.AreEqual(false, new Formula("5.0 + x5") != (new Formula("5.000 + x5")));

            Assert.AreEqual(true, new Formula("5.0 + x5") != (new Formula("5.00 + X5")));
        }

        /// <summary>
        /// Test getHashCode
        /// </summary>
        [TestMethod()]
        public void HashCodeTest()
        {
            Formula test = new Formula("10+5");
            Formula test2 = new Formula("2+2");

            Assert.AreEqual(true, test.GetHashCode() != test2.GetHashCode());

            Assert.AreEqual(false, new Formula("x1+y2").GetHashCode() != (new Formula("x1  +  y2")).GetHashCode());

            Assert.AreEqual(true, new Formula("x1+y2").GetHashCode() != (new Formula("y2+x1")).GetHashCode());

            Assert.AreEqual(false, new Formula("5.0 + x5").GetHashCode() != (new Formula("5.000 + x5")).GetHashCode());

            Assert.AreEqual(true, new Formula("5.0 + x5").GetHashCode() != (new Formula("5.00 + X5")).GetHashCode());

            Assert.AreEqual(true, new Formula("x1+y2").GetHashCode() == (new Formula("x1  +  y2")).GetHashCode());

            Assert.AreEqual(false, new Formula("x1+y2").GetHashCode() == (new Formula("y2+x1")).GetHashCode());

            Assert.AreEqual(true, new Formula("5.0 + x5").GetHashCode() == (new Formula("5.000 + x5")).GetHashCode());

            Assert.AreEqual(false, new Formula("5.0 + x5").GetHashCode() == (new Formula("5.00 + X5")).GetHashCode());
        }

        /// <summary>
        /// Test toString()
        /// </summary>
        [TestMethod()]
        public void toStringTest1()
        {
            Formula test = new Formula("10+5");
            Formula test2 = new Formula("2+2");

            Assert.AreEqual(false, test.ToString() == "Random");

            Assert.AreEqual(true, test.ToString() == "10+5");

            Assert.AreEqual(false, test.ToString() == test2.ToString());

            Assert.AreEqual(true, new Formula("x1+y2").ToString() == (new Formula("x1  +  y2")).ToString());

            Assert.AreEqual(false, new Formula("x1+y2").ToString() == (new Formula("y2+x1")).ToString());

            Assert.AreEqual(true, new Formula("5.0 + x5").ToString() == (new Formula("5.000 + x5")).ToString());

            Assert.AreEqual(false, new Formula("5.0 + x5").ToString() == (new Formula("5.00 + X5")).ToString());
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest5()
        {
            Formula test = new Formula("/8-9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest6()
        {
            Formula test = new Formula(")+8-9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest7()
        {
            Formula test = new Formula("(8-9)-");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest8()
        {
            Formula test = new Formula("8-+9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest9()
        {
            Formula test = new Formula("8-9 8");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest10()
        {
            Formula test = new Formula("8+x", s => s, s=> false);
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest11()
        {
            Formula test = new Formula("");
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void DivideByZeroTest1()
        {
            Assert.IsInstanceOfType(new Formula("10+2/0").Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void DivideByZeroTest2()
        {
            Assert.IsInstanceOfType(new Formula("(10+2)/(5-5)").Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void DivideByZeroTest3()
        {
            Assert.IsInstanceOfType(new Formula("(10+2)/(x5)").Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void DivideByZeroTest4()
        {
            Assert.IsInstanceOfType(new Formula("(x5/0)").Evaluate(s => 1), typeof(FormulaError));
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void DivideByZeroTest5()
        {
            Assert.IsInstanceOfType(new Formula("(12/x5)").Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest12()
        {
            Formula test = new Formula("8*");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest13()
        {
            Formula test = new Formula("*8");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest14()
        {
            Formula test = new Formula("*");
        }

        /// <summary>
        /// Test for undefined variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest15()
        {
            Formula test = new Formula("5+xx");
        }

        /// <summary>
        /// Test for undefined variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest16()
        {
            Formula test = new Formula("5+x1x");
        }

        /// <summary>
        /// Test for getVariables
        /// </summary>
        [TestMethod()]
        public void GetVariablesTest1()
        {
            int count = 0;
            Formula test = new Formula("x1 + x2 - y3 * y4");
            IEnumerable<string> temp = test.GetVariables();

            foreach (String s in temp)
                count++;

            Assert.AreEqual(count, 4);
        }

        /// <summary>
        /// Test for getVariables
        /// </summary>
        [TestMethod()]
        public void GetVariablesTest2()
        {
            Formula test = new Formula("x1 + x2 - y3 * y4");
            IEnumerable<string> temp = test.GetVariables();
            string[] testArray = new string[4] {"x1", "x2", "y3", "y4"};
            int i = 0;

            foreach (String s in temp)
            {
                Assert.AreEqual(s, testArray[i]);
                i++;
            }
        }

        /// <summary>
        /// Test for undefined variable
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest17()
        {
            Formula test = new Formula("8+x");
        }

        /// <summary>
        /// Large test for evaluate
        ///</summary>
        [TestMethod()]
        public void LargeEvaluateTest()
        {
            Assert.AreEqual(13, new Formula("x5").Evaluate(s => 13));
            Assert.AreEqual(16, new Formula("x5+5-2").Evaluate(s => 13));
            Assert.AreEqual(36, new Formula("(x5+5)*2").Evaluate(s => 13));
            Assert.AreEqual(9,  new Formula("(x5+5)/2").Evaluate(s => 13));
            Assert.AreEqual(75, new Formula("(x5+y5+5)*3").Evaluate(s => 10));
            Assert.AreEqual(16, new Formula("((x5+5)/5)*2+y5").Evaluate(s => 10));
            Assert.AreEqual(16, new Formula("((x5+5*1)/5-0)*2+y5").Evaluate(s => 10));
            Assert.AreEqual(11, new Formula("(20-10)+1").Evaluate(s => 0));
            Assert.AreEqual(9,  new Formula("(20-10)-1").Evaluate(s => 0));
            Assert.AreEqual(29, new Formula("(20+10)-1").Evaluate(s => 0));
            Assert.AreEqual(31, new Formula("(20+10)+1").Evaluate(s => 0));
            Assert.AreEqual(40, new Formula("(2*10)*2").Evaluate(s => 0));
            Assert.AreEqual(1,  new Formula("(20/10)/2").Evaluate(s => 0));
            Assert.AreEqual(15, new Formula("(20+10)/2").Evaluate(s => 0));
            Assert.AreEqual(3,  new Formula("2+2-1").Evaluate(s => 0));
            Assert.AreEqual(5,  new Formula("2+2+1").Evaluate(s => 0));
            Assert.AreEqual(-1, new Formula("2-2-1").Evaluate(s => 0));
            Assert.AreEqual(1,  new Formula("2-2+1").Evaluate(s => 0));
            Assert.AreEqual(2,  new Formula("(2*1)").Evaluate(s => 0));
            Assert.AreEqual(2,  new Formula("(2/1)").Evaluate(s => 0));
            Assert.AreEqual(5,  new Formula("(x5+1)+(x4*3)").Evaluate(s => 1));
            Assert.AreEqual(24, new Formula("(x5/2)*(x4*3)").Evaluate(s => 4));
            Assert.AreEqual(8,  new Formula("(16/x5)+(1*x6)").Evaluate(s => 4));
            Assert.AreEqual(2,  new Formula("(32/x5)/4").Evaluate(s => 4));
            Assert.AreEqual(8,  new Formula("(32/x5)/(x4/4)").Evaluate(s => 4));

            Formula test = new Formula("x5 + 6");
            Assert.AreEqual(8, test.Evaluate(s => 2));

            test = new Formula("6 + 6 + 2");
            Assert.AreEqual(14, test.Evaluate(x => 0));

            test = new Formula("6 + 6 + 2");
            Assert.AreEqual(14, test.Evaluate(x => 0));
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest18()
        {
            Formula test = new Formula("x5(");
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest19()
        {
            Formula test = new Formula("(2+1)x5");
        }

        /// <summary>
        /// Test for undefined variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest20()
        {
            Formula test = new Formula("5+1xx");
        }
    }
}


