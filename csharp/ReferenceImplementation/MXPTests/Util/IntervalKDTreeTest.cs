using System;
using System.Text;
using System.Collections.Generic;
using MXP.Util;
using NUnit.Framework;
using MXP.Messages;
using System.Diagnostics;

namespace MXPTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class IntervalKDTreeTest
    {
        private class TestBox
        {
            public double minX=0;
            public double minY=0;
            public double minZ=0;
            public double maxX=0;
            public double maxY=0;
            public double maxZ=0;
            public string value="";
        }

        [Test]
        public void CustomIntervalKDTreeTests()
        {
            IntervalKDTree<string> tree = new IntervalKDTree<string>(100, 10);
            tree.Put(-20,-20,-20,20,20,20,"test");

            HashSet<string> values=new HashSet<string>();
            tree.GetValues(-1, -21, -1, 1, -19, 1,values);
            Assert.AreEqual(1, values.Count);
        }

        /*
        [Test]
        public void TestIntervalKDTree()
        {
            IntervalKDTree<string> tree = new IntervalKDTree<string>(100, 10);

            IList<TestBox> testBoxes = new List<TestBox>();
            for (double i = -100; i < 100; i += 0.3)
            {
                TestBox box = new TestBox();
                box.minX = i;
                box.minY = i;
                box.minZ = i;
                box.maxX = i + 1;
                box.maxY = i + 2;
                box.maxZ = i + 3;
                box.value = "test" + i;
                testBoxes.Add(box);
                tree.Put(box.minX, box.minY, box.minZ, box.maxX, box.maxY, box.maxZ, box.value);
            }

            HashSet<string> foundValues;

            {
                double x1 = -101;
                double y1 = -101;
                double z1 = -101;
                for (double x2 = -100; x2 < 100; x2++)
                {
                    double y2 = x2 + 4;
                    double z2 = x2 + 5;

                    IList<TestBox> matchingBoxes = new List<TestBox>();
                    foreach (TestBox box in testBoxes)
                    {
                        if (
                            (x1 <= box.minX && box.minX < x2 &&
                             y1 <= box.minY && box.minY < y2 &&
                             z1 <= box.minZ && box.minZ < z2)
                            ||
                            (x1 <= box.maxX && box.maxX < x2 &&
                             y1 <= box.maxY && box.maxY < y2 &&
                             z1 <= box.maxZ && box.maxZ < z2)
                            ||
                            (box.minX <= x1 && x1 < box.maxX &&
                             box.minY <= y1 && y1 < box.maxY &&
                             box.minZ <= z1 && z1 < box.maxZ)
                            ||
                            (box.minX <= x2 && x2 < box.maxX &&
                             box.minY <= y2 && y2 < box.maxY &&
                             box.minZ <= z2 && z2 < box.maxZ)
                            )
                        {
                            matchingBoxes.Add(box);
                        }
                    }

                    foundValues = tree.GetValues(x1, y1, z1, x2, y2, z2, new HashSet<string>());
                    Assert.AreEqual(matchingBoxes.Count, foundValues.Count);
                }
            }

            foreach (TestBox box in testBoxes)
            {
                box.minX = -0.5;
                box.minY = -1.2;
                box.minY = -4.2;
                box.maxX =  3.2;
                box.maxY =  4.2;
                box.maxZ =  0.2;
                tree.Put(box.minX, box.minY, box.minZ, box.maxX, box.maxY, box.maxZ, box.value);
            }

            {
                double x1 = -101;
                double y1 = -101;
                double z1 = -101;
                for (double x2 = -100; x2 < 100; x2++)
                {
                    double y2 = x2 + 4;
                    double z2 = x2 + 5;

                    IList<TestBox> matchingBoxes = new List<TestBox>();
                    foreach (TestBox box in testBoxes)
                    {
                        if (
                            (x1 <= box.minX && box.minX < x2 &&
                             y1 <= box.minY && box.minY < y2 &&
                             z1 <= box.minZ && box.minZ < z2)
                            ||
                            (x1 <= box.maxX && box.maxX < x2 &&
                             y1 <= box.maxY && box.maxY < y2 &&
                             z1 <= box.maxZ && box.maxZ < z2)
                            ||
                            (box.minX <= x1 && x1 < box.maxX &&
                             box.minY <= y1 && y1 < box.maxY &&
                             box.minZ <= z1 && z1 < box.maxZ)
                            ||
                            (box.minX <= x2 && x2 < box.maxX &&
                             box.minY <= y2 && y2 < box.maxY &&
                             box.minZ <= z2 && z2 < box.maxZ)
                            )
                        {
                            matchingBoxes.Add(box);
                        }
                    }

                    foundValues = tree.GetValues(x1, y1, z1, x2, y2, z2, new HashSet<string>());
                    Assert.AreEqual(matchingBoxes.Count, foundValues.Count);
                }
            }

            foreach (TestBox cube in testBoxes)
            {
                tree.Remove(cube.value);
            }

            Assert.AreEqual(0, tree.GetValues(-100, -100, -100, 100, 100, 100, new HashSet<string>()).Count);

        }
*/
    }
}
