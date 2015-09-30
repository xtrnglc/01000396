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
            SpreadSheet s = new SpreadSheet();
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
            SpreadSheet s = new SpreadSheet();
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
            SpreadSheet s = new SpreadSheet();
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
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("A1", new Formula("5+10+A1"));
        }

        /// <summary>
        /// Test for  circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependecyTest2()
        {
            SpreadSheet s = new SpreadSheet();
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
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("A1", new Formula("4+B1"));
            s.SetCellContents("B1", new Formula("19+A1"));
        }

        /// <summary>
        /// Test for constructors
        /// </summary>
        [TestMethod]
        public void constructorTest1()
        {
            SpreadSheet s = new SpreadSheet();
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
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest2()
        {
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("2A1", "Hello");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest3()
        {
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("$A1", "Hello");
        }
    }
    
}
