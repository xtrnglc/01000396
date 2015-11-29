using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgCubio;
using NetworkController;

namespace ModelNetworkTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Constructor test for Cube
        /// </summary>
        [TestMethod]
        public void CubeConstructorTest1()
        {
            Cube c = new Cube(1, 2, 3, 4, 5, true, "test", 6);
            Assert.AreEqual(1, c.GetX());
            Assert.AreEqual(2, c.GetY());
            Assert.AreEqual(1, c.loc_x);
            Assert.AreEqual(2, c.loc_y);
            Assert.AreEqual(3, c.argb_color);
            Assert.AreEqual(4, c.uid);
            Assert.AreEqual(5, c.team_id);
            Assert.AreEqual(true, c.food);
            Assert.AreEqual("test", c.Name);
            Assert.AreEqual(6, c.Mass);
            Assert.AreEqual(c.GetColor(), 3);
            Assert.AreEqual(c.GetFood(), true);
            Assert.AreEqual(c.GetID(), 4);
            Assert.AreEqual(c.GetMass(), 6);
            Assert.AreEqual(c.GetName(), "test");
            Assert.AreEqual(c.GetWidth(), (int)Math.Sqrt(c.Mass));
        }

        /// <summary>
        /// Constructor test for World
        /// </summary>
        [TestMethod]
        public void WorldConstructorTest1()
        {
            World w = new World();
            Assert.AreEqual(1000, w.GetHeight);
            Assert.AreEqual(1000, w.GetWidth);
            Assert.AreEqual(2000, w.maxFood);
            Assert.AreEqual(500, w.topSpeed);
            Assert.AreEqual(10, w.attritionRate);
            Assert.AreEqual(1, w.foodValue);
            Assert.AreEqual(100, w.minimumSplitMass);
            Assert.AreEqual(10, w.maximumSplits);
            
        }

        /// <summary>
        /// Constructor test for World
        /// </summary>
        [TestMethod]
        public void WorldConstructorTest2()
        {
            World w = new World(1000, 1000, 2000, 500, 10, 1, 500, 100, 10);
            Assert.AreEqual(1000, w.GetHeight);
            Assert.AreEqual(1000, w.GetWidth);
            Assert.AreEqual(2000, w.maxFood);
            Assert.AreEqual(500, w.topSpeed);
            Assert.AreEqual(10, w.attritionRate);
            Assert.AreEqual(1, w.foodValue);
            Assert.AreEqual(500, w.startMass);
            Assert.AreEqual(100, w.minimumSplitMass);
            Assert.AreEqual(10, w.maximumSplits);
        }

        /// <summary>
        /// Add method test
        /// </summary>
        [TestMethod]
        public void WorldAddTest()
        {
            World w = new World();
            Cube c1 = new Cube(1, 2, 3, 4, 5, false, "player", 6);
            Cube c2 = new Cube(7, 8, 9, 10, 11, true, "food", 12);
            w.Add(c1);
            w.Add(c2);
            Cube c3;
            Cube c4;
            w.ListOfPlayers.TryGetValue(c1.GetID(), out c3);
            w.ListOfFood.TryGetValue(c2.GetID(), out c4);
            Assert.AreEqual(c1, c3);
            Assert.AreEqual(c2, c4);
        }
    }
}
