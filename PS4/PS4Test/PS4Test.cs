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
            s.SetContentsOfCell("A1", "10");
            Assert.AreEqual(s.GetCellValue("A1"), 10.0);

            s.SetContentsOfCell("B1", "10.2");
            Assert.AreEqual(s.GetCellValue("B1"), 10.2);

            s.SetContentsOfCell("A1", "19");
            Assert.AreNotEqual((double)s.GetCellValue("A1"), 10.0);
            Assert.AreEqual((double)s.GetCellValue("A1"), 19.0);
        }

        /// <summary>
        /// Test for initializing and getting cell string contents
        /// </summary>
        [TestMethod]
        public void stringContentTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual((string)s.GetCellContents("A1"), "Hello");

            s.SetContentsOfCell("B1", "World");
            Assert.AreEqual((string)s.GetCellContents("B1"), "World");

            s.SetContentsOfCell("A1", "Not hello");
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
            s.SetContentsOfCell("A1", ("=5+10"));
            Assert.AreEqual(f.Evaluate(x => 0), s.GetCellValue("A1"));
        }

        /// <summary>
        /// Test for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=5+10+A1"));
        }

        /// <summary>
        /// Test for  circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=4+C1"));
            s.SetContentsOfCell("B1", ("=19+D1"));
            s.SetContentsOfCell("C1", ("=B1+2"));
            s.SetContentsOfCell("D1", ("=1+A1"));
        }

        /// <summary>
        /// Test for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", ("=19+A1"));
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("D1", ("=13+A1"));


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
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("B1", ("=13+E1"));
            s.SetContentsOfCell("E1", "21.0");


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[4] { "A1", "B1", "C1", "E1" };
            object[] testArray = new object[4] { ("4+B1"), ("13+E1"), "hello", "21" };
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
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", ("=10+B1"));
            s.SetContentsOfCell("B1", "12.0");


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[3] { "A1", "B1", "C1" };
            object[] testArray = new object[3] { ("4+B1"), "12", ("10+B1"), };
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
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("A1", "12.0");
            s.SetContentsOfCell("E1", "21.0");


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[4] { "A1", "B1", "C1", "E1" };
            object[] testArray = new object[4] { "12", "10", "hello", "21" };
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
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("B1", ("=A1*2"));
            s.SetContentsOfCell("C1", ("=B1+A1"));
            s.SetContentsOfCell("A1", "12.0");


            IEnumerable<string> temp = s.GetNamesOfAllNonemptyCells();
            string[] nameArray = new string[3] { "A1", "B1", "C1" };
            object[] testArray = new object[3] { "12", ("A1*2"), ("B1+A1") };
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
            s.SetContentsOfCell("2A1", "Hello");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("$A1", "Hello");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        public void stringContentTest4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
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
            s.SetContentsOfCell("2A1A", "12");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void formulaContentTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("2A2", "5");
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
            s.SetContentsOfCell("A1", empty);
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
            s.SetContentsOfCell(null, "=1+2");
        }

        /// <summary>
        /// Tests invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("4356", "42");

        }

        /// <summary>
        /// Test for empty text string
        /// </summary>
        [TestMethod]
        public void emptyStringTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "");
            sheet.SetContentsOfCell("B1", "20");
            sheet.SetContentsOfCell("C1", "String");
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
            
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("D1", "=A1+B1");
            s.SetContentsOfCell("A1", "=A3*B4");
            s.SetContentsOfCell("A3", "=A1");
        }

        /// <summary>
        /// Test for circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDepedencyTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", ("=B1*2"));
            s.SetContentsOfCell("B1", ("=C1*2"));
            s.SetContentsOfCell("C1", ("=A1*2"));
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
