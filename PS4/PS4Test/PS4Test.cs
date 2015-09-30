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

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest2()
        {
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("2A1", "Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void stringContentTest3()
        {
            SpreadSheet s = new SpreadSheet();
            s.SetCellContents("$A1", "Hello");
        }
    }
    
}
