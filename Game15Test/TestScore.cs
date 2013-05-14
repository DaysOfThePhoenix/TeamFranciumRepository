using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameFifteen;

namespace Game15Test
{
    [TestClass]
    public class TestScore
    {
        [TestMethod]
        public void TestConstructor_ValidParameters()
        {
            Score scoreForTest = new Score("testName", 200, 4, "testFileName.txt");
            Assert.AreEqual(scoreForTest.Name, "testName");
            Assert.AreEqual(scoreForTest.Points, 200);
            Assert.AreEqual(scoreForTest.TopScoresCount, 4);
            Assert.AreEqual(scoreForTest.FileNameForExternalSave, "testFileName.txt");
        }
    }
}
