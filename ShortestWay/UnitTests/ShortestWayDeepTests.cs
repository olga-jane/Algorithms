using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShortestWay;

namespace UnitTests
{
    [TestClass]
    public class ShortestWayDeepTests
    {
        [TestMethod]
        public void DeepSimpleTest()
        {
            byte[,] testField = new byte[,]
            {
                { 0,  (6),   0, },  // x == 0
                { 5,  0,     2, },  // x == 1
                { 0,  (100), 1, },  // x == 2
            };

            World world = new World(3, 3, (x, y) => { return testField[x, y]; });
            {
                Location[] loc = world.FindShortestWay(new Location(0, 1), new Location(2, 1), new QuickPathDeepSearch(isDiagonaleCrossing: true));
                Assert.IsNotNull(loc);
                Assert.AreEqual(3, loc.Length);
                Assert.IsTrue(LocationHelper.AreEqual(new Location(0, 1), loc[0]));
                Assert.IsTrue(LocationHelper.AreEqual(new Location(1, 0), loc[1]));
                Assert.IsTrue(LocationHelper.AreEqual(new Location(2, 1), loc[2]));
            }
            {
                Location[] loc = world.FindShortestWay(new Location(0, 1), new Location(2, 1), new QuickPathDeepSearch(isDiagonaleCrossing: false));
                Assert.AreEqual(0, loc.Length);
            }
        }
        [TestMethod]
        public void DeepNoDeadEndPathTest()
        {
            byte[,] testField = new byte[,]
            {
                { (1),  100,  0, }, // x == 0
                { 1,    0,    0, }, // x == 1
                { 0,    1,    (1), }, // x == 2
            };

            World world = new World(3, 3, (x, y) => { return testField[x, y]; });

            Location[] loc = world.FindShortestWay(new Location(0, 0), new Location(2, 2), new QuickPathDeepSearch(isDiagonaleCrossing: true));
            Assert.IsNotNull(loc);
            Assert.AreEqual(4, loc.Length);
            Assert.IsTrue(LocationHelper.AreEqual(new Location(0, 0), loc[0]));
            Assert.IsTrue(LocationHelper.AreEqual(new Location(1, 0), loc[1]));
            Assert.IsTrue(LocationHelper.AreEqual(new Location(2, 1), loc[2]));
            Assert.IsTrue(LocationHelper.AreEqual(new Location(2, 2), loc[3]));
        }

        [TestMethod]
        public void DeepComplicatedTest()
        {
            byte[,] testField = new byte[,]
            {
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,     5 },
                { 0, 40 /**/, 5, 3, 6, 2, 1, 2, 6,     5 },
                { 0, 0,       0,  3, 6, 2, 1, 0, 6,    5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 60,    5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,     5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,     5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,/**/ 5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,     5 },
                { 0, 0,       0, 3, 6, 2, 1, 1, 6,     5 },
                { 0, 0,       0, 3, 6, 2, 1, 0, 6,     5 },
            };

            World world = new World(10, 10, (x, y) => { return testField[x, y]; });

            Location[] loc = world.FindShortestWay(new Location(1, 1), new Location(6, 8), new QuickPathDeepSearch(isDiagonaleCrossing: true));
            Assert.IsNotNull(loc);
            Assert.AreEqual(12, loc.Length);
        }

        [TestMethod]
        public void DiagonaleTest()
        {
            byte[,] testField = new byte[,]
            {
            { 0,  1,  4,  9, 2, 100 },
            { 0,  5,  3,  8, 6, 99 },
            { 10, 1,  14, 9, 7, 98 },
            { 12, 11, 15, 0, 17, 97 },
            { 13,  1, 0,  16, 18, 96 },
            { 0,  22, 21, 20, 19, 95 },
            };
            World world = new World(testField.GetLength(0), testField.GetLength(1), (x, y) => { return testField[x, y]; });

            Location[] loc = world.FindShortestWay(new Location(1, 1), new Location(5, 4), new QuickPathDeepSearch(isDiagonaleCrossing: true));
            Assert.IsNotNull(loc);
            Assert.AreEqual(5, loc.Length);
        }

        [TestMethod]
        public void BypassTest()
        {
            byte[,] testField = new byte[,]
            {
            { 0,  1, 94, 93, 92, 100 },
            { 0,  5,  3,  8, 6, 99 },
            { 10, 1,  14, 9, 7, 98 },
            { 12, 11, 15, 0, 17, 97 },
            { 13,  1, 0,  16, 18, 96 },
            { 0,  22, 21, 20, 19, 95 },
            };
            World world = new World(testField.GetLength(0), testField.GetLength(1), (x, y) => { return testField[x, y]; });

            Location[] loc = world.FindShortestWay(new Location(1, 1), new Location(5, 4), new QuickPathDeepSearch(isDiagonaleCrossing: true));
            Assert.IsNotNull(loc);
            Assert.AreEqual(9, loc.Length);
        }
    }
    }



