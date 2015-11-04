using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace SpreadSheetGUITest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        [TestMethod]
        public void SetCellContentTest1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.SetCellContent1();
            this.UIMap.AssertMethod1();
           
        }

        [TestMethod]
        public void SetCellContentTest2()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.SetCellContent2();
            this.UIMap.AssertMethod2();
            this.UIMap.AssertMethod3();

        }

        [TestMethod]
        public void SetCellContentTest3()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.SetCellContent3();
            this.UIMap.AssertMethod8();
            this.UIMap.AssertMethod9();
        }

        [TestMethod]
        public void CircularExceptionTest1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.CircularException();
            this.UIMap.AssertMethod11();
            this.UIMap.AssertMethod12();
        }

        [TestMethod]
        public void SavingTest1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.CheckUpdating2();
            this.UIMap.AssertMethod18();
            this.UIMap.CheckUpdating3();
            this.UIMap.AssertMethod19();
        }


        [TestMethod]
        public void SetCellContentTest4()
        {
            this.UIMap.TestForString1();
            this.UIMap.CheckCellValueString();
            this.UIMap.CheckCellContentTypeString();
        }

        [TestMethod]
        public void SetCellContentTest5()
        {
            this.UIMap.Step1();
            this.UIMap.Step1Test1();
            this.UIMap.Step2();
            this.UIMap.Step2Test1();
            this.UIMap.Step3();
            this.UIMap.Step3Test1();
        }

        [TestMethod]
        public void CircularErrorTest2()
        {
            this.UIMap.Step1_1();
            this.UIMap.Step1Test1_1();
        }

        [TestMethod]
        public void SumTest3()
        {
            this.UIMap.Step1_3();
            this.UIMap.Step2_3();
            this.UIMap.Step2Test1_3();
        }


        [TestMethod]
        public void SumTest1()
        {
            this.UIMap.SumTestMethod1();
            this.UIMap.CheckingSum1();
        }

        [TestMethod]
        public void SumTest2()
        {
            this.UIMap.SumTestMethod2();
            this.UIMap.CheckingSum2();
        }

        [TestMethod]
        public void NewTest1()
        {
            this.UIMap.NewStep1();
            this.UIMap.NewStep1Test1();
            this.UIMap.NewStep1Test2();
        }

        [TestMethod]
        public void SaveTest2()
        {
            this.UIMap.SaveTestStep1();
            this.UIMap.SaveTestStep1Test1();
        }
        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
