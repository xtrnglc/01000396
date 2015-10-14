/*
Author: Trung Le
Date: 09/29/2015
Purpose: Console test used mainly for debugging
*/
using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace PS4Test
{
    [TestClass]
    public class PS4Test
    {
        /// <summary>
        /// Test for initializing and getting cell double contents
        /// </summary>
        [TestMethod]
        public void doubleContentTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 10);
            Assert.AreEqual((double)s.GetCellContents("A1"), 10);

            s.SetCellContents("B1", 10.2);
            Assert.AreEqual((double)s.GetCellContents("B1"), 10.2);

            s.SetCellContents("A1", 19);
            Assert.AreNotEqual((double)s.GetCellContents("A1"), 10);
            Assert.AreEqual((double)s.GetCellContents("A1"), 19);
        }

        /// <summary>
        /// Test for initializing and getting cell string contents
        /// </summary>
        [TestMethod]
        public void stringContentTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Hello");
            Assert.AreEqual((string)s.GetCellContents("A1"), "Hello");

            s.SetCellContents("B1", "World");
            Assert.AreEqual((string)s.GetCellContents("B1"), "World");

            s.SetCellContents("A1", "Not hello");
            Assert.AreNotEqual((string)s.GetCellContents("A1"), "Hello");
            Assert.AreEqual((string)s.GetCellContents("A1"), "Not hello");
        }

        /// <summary>
        /// Test for initializing and getting cell string contents
        /// </summary>
        [TestMethod]
        public void formulaContentTest1()
        {
            Formula f = new Formula("5+10");
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("5+10"));
            Assert.AreEqual(f, s.GetCellContents("A1"));
        }

        /// <summary>
        /// Test for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("5+10+A1"));
        }

        /// <summary>
        /// Test for  circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+C1"));
            s.SetCellContents("B1", new Formula("19+D1"));
            s.SetCellContents("C1", new Formula("B1+2"));
            s.SetCellContents("D1", new Formula("1+A1"));
        }

        /// <summary>
        /// Test for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", new Formula("19+A1"));
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", 10);
            s.SetCellContents("C1", "hello");
            s.SetCellContents("D1", new Formula("13+A1"));


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] testArray = new string[4] { "A1", "B1", "C1", "D1"};
            int i = 0;

            foreach (String t in temp)
            {
                Assert.AreEqual(testArray[i], t);
                i++;
            }
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", 10);
            s.SetCellContents("C1", "hello");
            s.SetCellContents("B1", new Formula("13+E1"));
            s.SetCellContents("E1", 21.0);


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[4] { "A1", "B1", "C1", "E1" };
            object[] testArray = new object[4] { new Formula("4+B1"), new Formula("13+E1"), "hello", 21.0 };
            int i = 0;

            foreach (String t in nameArray)
            {
                Assert.AreEqual(testArray[i], s.GetCellContents(nameArray[i]));
                i++;
            }
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", 10);
            s.SetCellContents("C1", new Formula("10+B1"));
            s.SetCellContents("B1", 12.0);


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[3] { "A1", "B1", "C1" };
            object[] testArray = new object[3] { new Formula("4+B1"), 12.0, new Formula("10+B1"), };
            int i = 0;

            foreach (String t in nameArray)
            {
                Assert.AreEqual(testArray[i], s.GetCellContents(nameArray[i]));
                i++;
            }
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", 10);
            s.SetCellContents("C1", "hello");
            s.SetCellContents("A1", 12.0);
            s.SetCellContents("E1", 21.0);


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[4] { "A1", "B1", "C1", "E1" };
            object[] testArray = new object[4] { 12.0, 10.0, "hello", 21.0 };
            int i = 0;

            foreach (String t in nameArray)
            {
                Assert.AreEqual(testArray[i], s.GetCellContents(nameArray[i]));
                i++;
            }
        }

        /// <summary>
        /// Test for constructors
        /// 
        /// </summary>
        [TestMethod]
        public void constructorTest5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 10);
            s.SetCellContents("B1", new Formula("A1*2"));
            s.SetCellContents("C1", new Formula("B1+A1"));
            s.SetCellContents("A1", 12.0);


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[3] { "A1", "B1", "C1" };
            object[] testArray = new object[3] { 12.0, new Formula("A1*2"), new Formula("B1+A1") };
            int i = 0;

            foreach (String t in nameArray)
            {
                Assert.AreEqual(testArray[i], s.GetCellContents(nameArray[i]));
                i++;
            }
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("2A1", "Hello");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("$A1", "Hello");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        public void stringContentTest4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Hello");
            Assert.AreEqual(null, s.GetCellContents("A3"));
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void doubleContentTest4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("2A1A", 12);
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void formulaContentTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A2", "5");
        }

        /// <summary>
        /// Tests null exception 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullTest1()
        {
            String empty = null;
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", empty);
        }

        /// <summary>
        /// Tests null name exception 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        /// <summary>
        /// Tests null name exception 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod7()
        {
            Formula test = new Formula("1+51");
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, test);
        }

        /// <summary>
        /// Tests invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("4356", 42);

        }

        /// <summary>
        /// Test for empty text string
        /// </summary>
        [TestMethod]
        public void emptyStringTest()
        {
            Formula f1 = new Formula("C1+B1", x => x.ToUpper(), x => true);
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "");
            sheet.SetCellContents("B1", 20);
            sheet.SetCellContents("C1", "String");
            HashSet<string> s1 = new HashSet<string>(sheet.GetNamesOfAllNonemptyCells());
            HashSet<string> s2 = new HashSet<string>();
            s2.Add("B1");
            s2.Add("C1");
            Assert.AreEqual(s2.Count, s1.Count);
        }

        /// <summary>
        /// Test for circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDepedencyTest4()
        {
            Formula f1 = new Formula("A1+B1");
            Formula f2 = new Formula("A3*B4");
            Formula f3 = new Formula("E1+C1");
            Formula f4 = new Formula("C1-A3");
            Formula f5 = new Formula("A1");
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("D1", f1);
            s.SetCellContents("A1", f2);
            s.SetCellContents("B1", f2);
            s.SetCellContents("A3", f3);
            s.SetCellContents("B4", f4);
            s.SetCellContents("E1", 2);
            s.SetCellContents("C1", 6);
            s.SetCellContents("A3", f5);
        }

        /// <summary>
        /// Test for circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDepedencyTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetCellContents("A1", new Formula("B1*2"));
            s.SetCellContents("B1", new Formula("C1*2"));
            s.SetCellContents("C1", new Formula("A1*2"));
        }

        /// <summary>
        /// Test for empty cell being requested to return cell content
        /// </summary>
        [TestMethod]
        public void nullTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            Assert.AreEqual(s.GetCellContents("D1"), null);
        }


    }

}
