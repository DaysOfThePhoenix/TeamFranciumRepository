using System;
using GameFifteen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game15Test
{
    [TestClass]
    public class TestBoard
    {
        [TestMethod]
        public void TestBoardConstructorMatrixSize()
        {
            Board board = new Board(3, 6);

            Assert.AreEqual(board.MatrixSizeRows, 3);
            Assert.AreEqual(board.MatrixSizeColumns, 6);
        }
    }
}
