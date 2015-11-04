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
        public void SquareTest1()
        {
            this.UIMap.SquareTest1();
            this.UIMap.SquareTest1_assert();
        }

        [TestMethod]
        public void SquareRootTest1()
        {
            this.UIMap.SquareRootTest1();
            this.UIMap.SquareRootTest1_assert();
        }

        [TestMethod]
        public void SumTest4()
        {
            this.UIMap.SumTestMethod3();
            this.UIMap.SumTestMethod3_assert();
        }

        [TestMethod]
        public void SaveTest2()
        {
            this.UIMap.SaveTestStep1();
            this.UIMap.SaveTestStep1Test1();
        }

        [TestMethod]
        public void SaveTest3()
        {
            this.UIMap.SaveTest1Step1();
            this.UIMap.SaveTest1Step1_assert();
            this.UIMap.SaveTest1Step1_assert2();
        }

        [TestMethod]
        public void SquareRootTest2()
        {
            this.UIMap.SquareRootTest2();
            this.UIMap.SquareRootAssert1();
            this.UIMap.ClosingMethod1();
        }

        [TestMethod]
        public void SavingTest2()
        {
            this.UIMap.Saving1();
            this.UIMap.SavingAssertion1();
            this.UIMap.Saving2();
            this.UIMap.SavingAssertion2();
            this.UIMap.Saving3();
            this.UIMap.SavingAssertion3();
            this.UIMap.ClosingMethod1();
        }

        [TestMethod]
        public void SavingTest3()
        {
            this.UIMap.Saving5();
        }

        [TestMethod]
        public void OpenTest1()
        {
            this.UIMap.OpenMethod1();
            this.UIMap.OpenAssert1();
            this.UIMap.OpenAssert2();
            this.UIMap.OpenMethod2();
            this.UIMap.OpenAssert3();
        }

        [TestMethod]
        public void AverageTest1()
        {
            this.UIMap.AverageMethod1();
            this.UIMap.AverageAssert1();
        }

        [TestMethod]
        public void SavingTest4()
        {
            this.UIMap.SavingMethod1();
        }

        [TestMethod]
        public void HelpMenuTest()
        {
            this.UIMap.HelpMethod1();
            this.UIMap.HelpAssert1();
            this.UIMap.HelpMethod2();
            this.UIMap.HelpAssert2();
            this.UIMap.HelpMethod3();
            this.UIMap.HelpAssert3();
            this.UIMap.HelpMethod4();
            this.UIMap.HelpAssert4();
            this.UIMap.HelpMethod5();
            this.UIMap.HelpAssert5();
            this.UIMap.HelpMethod6();
            this.UIMap.HelpAssert6();
            this.UIMap.HelpMethod7();
            this.UIMap.HelpAssert7();
            this.UIMap.HelpMethod8();
            this.UIMap.HelpAssert8();
            this.UIMap.HelpMethod9();
            this.UIMap.HelpAssert9();
            this.UIMap.HelpMethod10();
            this.UIMap.HelpAssert10();
            this.UIMap.HelpMethod11();
            this.UIMap.HelpAssert11();
            this.UIMap.HelpMethod12();
            this.UIMap.HelpAssert12();
            this.UIMap.HelpMethod13();
            this.UIMap.HelpAssert13();
            this.UIMap.HelpMethod14();
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
