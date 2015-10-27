/*
Author: Trung Le
Date: 09/29/2015
Purpose: Console test used mainly for debugging
PS5 BRANCH
*/
using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.IO;
using System.Text.RegularExpressions;

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
        /// Test for initializing and getting cell double contents
        /// </summary>
        [TestMethod]
        public void test4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=&");
            
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
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", ("=10+B1"));
            s.SetContentsOfCell("B1", "12.0");


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
            s.SetContentsOfCell("A1", ("=4+B1"));
            s.SetContentsOfCell("B1", "10");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("A1", "12.0");
            s.SetContentsOfCell("E1", "21.0");


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
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("B1", ("=A1*2"));
            s.SetContentsOfCell("C1", ("=B1+A1"));
            s.SetContentsOfCell("A1", "12.0");


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

        /// <summary>
        /// Test for empty cell being requested to return cell value
        /// </summary>
        [TestMethod]
        public void nullTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            Assert.AreEqual(s.GetCellValue("D1"), null);
        }

        /// <summary>
        /// Test for empty cell being requested to return cell value
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullTest4()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            Assert.AreEqual(s.GetCellValue(null), null);
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("2D", "=2+M2");
        }

        /// <summary>
        /// Test for invalid naming practices
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidFormulaTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("D2", "=2++M2");
        }

        /// <summary>
        /// Test for three argument constructor
        /// </summary>
        [TestMethod]
        public void constructorTest6()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "2.1");
            s.SetContentsOfCell("D2", "=2+M2");
            s.SetContentsOfCell("M2", "12");
            Assert.AreEqual(s.GetCellValue("M2"), 12.0);
            Assert.AreEqual(s.GetCellValue("D2"), 14.0);

        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        public void SaveTest()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.0");
            s.Save("save1.txt");
            Assert.AreEqual("1.0", new Spreadsheet().GetSavedVersion("save1.txt"));
        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        public void SaveTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
            s.SetContentsOfCell("D1", "Hello World");
            s.Save("save2.txt");
            Assert.AreEqual("1.1", new Spreadsheet().GetSavedVersion("save2.txt"));
        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
            s.SetContentsOfCell("D1", "Hello World");
            s.Save("");
        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
            s.SetContentsOfCell("D1", "Hello World");
            new Spreadsheet().GetSavedVersion("ducky");
        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest4()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
            s.SetContentsOfCell("D1", "Hello World");
            new Spreadsheet().GetSavedVersion("");
        }

        /// <summary>
        /// Test for save method
        /// </summary>
        [TestMethod()]
        public void SaveTest5()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
            s.SetContentsOfCell("D1", "Hello World");
            Assert.AreEqual(s.Changed, true);
            s.Save("save3.txt");
            Assert.AreEqual(s.Changed, false);
        }

        /// <summary>
        /// Test for validator and normalizer
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructorTest7()
        {
            AbstractSpreadsheet s = new Spreadsheet(validator2, normalizer2, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=E1+2");
        }

        /// <summary>
        /// Test for validator and normalizer
        /// </summary>
        [TestMethod()]
        public void constructorTest8()
        {
            AbstractSpreadsheet s = new Spreadsheet(validator2, normalizer2, "1.1");
            s.SetContentsOfCell("E1", "21.0");
            s.SetContentsOfCell("A1", "=ee5+2");
        }

        /// <summary>
        /// Test for save
        /// </summary>
        [TestMethod()]
        public void SaveTest7()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("A1", "21.0");
            s.SetContentsOfCell("B1", "13+A1");
            s.SetContentsOfCell("C1", "Hello World");
            s.Save("save8.txt");
            AbstractSpreadsheet ss = new Spreadsheet("save8.txt", x => true, x => x, "1.1");
            Assert.AreEqual(ss.GetCellContents("A1"), s.GetCellContents("A1"));
            Assert.AreEqual(ss.GetCellContents("B1"), s.GetCellContents("B1"));
            Assert.AreEqual(ss.GetCellContents("C1"), s.GetCellContents("C1"));

            Assert.AreEqual(ss.GetCellValue("A1"), s.GetCellValue("A1"));
            Assert.AreEqual(ss.GetCellValue("B1"), s.GetCellValue("B1"));
            Assert.AreEqual(ss.GetCellValue("C1"), s.GetCellValue("C1"));
        }

        /// <summary>
        /// Test for save
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest8()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("A1", "21.0");
            s.SetContentsOfCell("B1", "13+A1");
            s.SetContentsOfCell("C1", "Hello World");
            s.Save("save8.txt");
            AbstractSpreadsheet ss = new Spreadsheet("WOOHOO.txt", x => true, x => x, "1.1");
        }

        /// <summary>
        /// Test for save
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest9()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "1.1");
            s.SetContentsOfCell("A1", "21.0");
            s.SetContentsOfCell("B1", "13+A1");
            s.SetContentsOfCell("C1", "Hello World");
            s.Save("save8.txt");
            AbstractSpreadsheet ss = new Spreadsheet("save8.txt", x => true, x => x, "2.3");
        }



        /// <summary>
        /// Normalizer test, returns X
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string normalizer1(string s)
        {
            if (s == "y2")
            {
                return "x5";
            }

            else if (s == "z2")
            {
                return "y5";
            }

            else if (s == "w2")
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
        private string normalizer2(string s)
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
