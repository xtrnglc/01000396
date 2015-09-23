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
        public void TestMethod1()
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
        public void TestMethod2()
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
        public void TestMethod3()
        {
            Formula test = new Formula("((4+1)))");
        }

        /// <summary>
        /// Test for unequal parentheses, more opening
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod4()
        {
            Formula test = new Formula("(((4+1))");
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod5()
        {
            Formula test = new Formula("8(");
        }

        /// <summary>
        /// Test for unbalaned parentheses
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod6()
        {
            Formula test = new Formula("(8");
        }

        /// <summary>
        /// Test .Equals
        /// </summary>
        [TestMethod()]
        public void TestMethod7()
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
        public void TestMethod8()
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
        public void TestMethod9()
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
        public void TestMethod10()
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
        public void TestMethod11()
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
        public void TestMethod12()
        {
            Formula test = new Formula("/8-9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod13()
        {
            Formula test = new Formula(")+8-9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod14()
        {
            Formula test = new Formula("(8-9)-");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod15()
        {
            Formula test = new Formula("8-+9");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod16()
        {
            Formula test = new Formula("8-9 8");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod17()
        {
            Formula test = new Formula("8+x", s => s, s=> false);
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod18()
        {
            Formula test = new Formula("");
        }

        /// <summary>
        /// Test for divide by zero
        ///</summary>
        [TestMethod()]
        public void TestMethod19()
        {
            Assert.IsInstanceOfType(new Formula("10+2/0").Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod20()
        {
            Formula test = new Formula("8*");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod21()
        {
            Formula test = new Formula("*8");
        }

        /// <summary>
        /// Test for invalid syntax
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod22()
        {
            Formula test = new Formula("*");
        }

        /// <summary>
        /// Test for undefined variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod23()
        {
            Formula test = new Formula("5+xx");
        }

        /// <summary>
        /// Test for undefined variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMethod24()
        {
            Formula test = new Formula("5+x1x");
        }

        /// <summary>
        /// Test for getVariables
        /// </summary>
        [TestMethod()]
        public void TestMethod25()
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
        public void TestMethod26()
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
        public void TestMethod27()
        {
            Formula test = new Formula("8+x");
        }

        /// <summary>
        /// Large test for evaluate
        ///</summary>
        [TestMethod()]
        public void TestMethod28()
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
            Assert.AreEqual(3,  new Formula("2+2-1").Evaluate(s => 0));
            Assert.AreEqual(5,  new Formula("2+2+1").Evaluate(s => 0));
            Assert.AreEqual(-1, new Formula("2-2-1").Evaluate(s => 0));
            Assert.AreEqual(1,  new Formula("2-2+1").Evaluate(s => 0));
            Assert.AreEqual(2,  new Formula("(2*1)").Evaluate(s => 0));
            Assert.AreEqual(2,  new Formula("(2/1)").Evaluate(s => 0));
            Assert.AreEqual(5,  new Formula("(x5+1)+(x4*3)").Evaluate(s => 1));
            Assert.AreEqual(24, new Formula("(x5/2)*(x4*3)").Evaluate(s => 4));
            Assert.AreEqual(8,  new Formula("(16/x5)+(1*x6)").Evaluate(s => 4));

            Formula test = new Formula("x5 + 6");
            Assert.AreEqual(8, test.Evaluate(s => 2));

            test = new Formula("6 + 6 + 2");
            Assert.AreEqual(14, test.Evaluate(x => 0));

            test = new Formula("6 + 6 + 2");
            Assert.AreEqual(14, test.Evaluate(x => 0));
        }








    }
}


/*
[TestMethod()]
        public void Test1()
        {
            Assert.AreEqual(5, Evaluator.Evaluate("5", s => 0));
        }

        [TestMethod()]
        public void Test2()
        {
            Assert.AreEqual(13, Evaluator.Evaluate("X5", s => 13));
        }

        [TestMethod()]
        public void Test3()
        {
            Assert.AreEqual(8, Evaluator.Evaluate("5+3", s => 0));
        }

        [TestMethod()]
        public void Test4()
        {
            Assert.AreEqual(8, Evaluator.Evaluate("18-10", s => 0));
        }

        [TestMethod()]
        public void Test5()
        {
            Assert.AreEqual(8, Evaluator.Evaluate("2*4", s => 0));
        }

        [TestMethod()]
        public void Test6()
        {
            Assert.AreEqual(8, Evaluator.Evaluate("16/2", s => 0));
        }

        [TestMethod()]
        public void Test7()
        {
            Assert.AreEqual(6, Evaluator.Evaluate("2+X1", s => 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test8()
        {
            Evaluator.Evaluate("2+X1", s => { throw new ArgumentException("Unknown variable"); });
        }

        [TestMethod()]
        public void Test9()
        {
            Assert.AreEqual(15, Evaluator.Evaluate("2*6+3", s => 0));
        }

        [TestMethod()]
        public void Test10()
        {
            Assert.AreEqual(20, Evaluator.Evaluate("2+6*3", s => 0));
        }

        [TestMethod()]
        public void Test11()
        {
            Assert.AreEqual(24, Evaluator.Evaluate("(2+6)*3", s => 0));
        }

        [TestMethod()]
        public void Test12()
        {
            Assert.AreEqual(16, Evaluator.Evaluate("2*(3+5)", s => 0));
        }

        [TestMethod()]
        public void Test13()
        {
            Assert.AreEqual(10, Evaluator.Evaluate("2+(3+5)", s => 0));
        }

        [TestMethod()]
        public void Test14()
        {
            Assert.AreEqual(50, Evaluator.Evaluate("2+(3+5*9)", s => 0));
        }

        [TestMethod()]
        public void Test15()
        {
            Assert.AreEqual(26, Evaluator.Evaluate("2+3*(3+5)", s => 0));
        }

        [TestMethod()]
        public void Test16()
        {
            Assert.AreEqual(194, Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", s => 0));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test17()
        {
            Evaluator.Evaluate("5/0", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test18()
        {
            Evaluator.Evaluate("+", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test19()
        {
            Evaluator.Evaluate("2+5+", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test20()
        {
            Evaluator.Evaluate("2+5*7)", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test21()
        {
            Evaluator.Evaluate("xx", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test22()
        {
            Evaluator.Evaluate("5+xx", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test23()
        {
            Evaluator.Evaluate("5+7+(5)8", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test24()
        {
            Evaluator.Evaluate("", s => 0);
        }

        [TestMethod()]
        public void Test25()
        {
            Assert.AreEqual(-12, Evaluator.Evaluate("y1*3-8/2+4*(8-9*2)/2*x7", s => (s == "x7") ? 1 : 4));
        }

        [TestMethod()]
        public void Test26()
        {
            Assert.AreEqual(6, Evaluator.Evaluate("x1+(x2+(x3+(x4+(x5+x6))))", s => 1));
        }

        [TestMethod()]
        public void Test27()
        {
            Assert.AreEqual(12, Evaluator.Evaluate("((((x1+x2)+x3)+x4)+x5)+x6", s => 2));
        }

        [TestMethod()]
        public void Test28()
        {
            Assert.AreEqual(0, Evaluator.Evaluate("a4-a4*a4/a4", s => 3));
        }

*/

