using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameFifteen;

namespace Game15Test
{
    [TestClass]
    public class TestEngine
    {
        [TestMethod]
        public void TestLoadBoard()
        {
            Board comparingBoard = new Board(4, 4);
            Board engineBoard = new Board(4, 4);
            Engine testEngine = new Engine(null, engineBoard, null);
            comparingBoard.InitializeMatrix();
            testEngine.LoadBoard();

            int differences = 0;
            int MinCountOfDifferences = 
                (comparingBoard.MatrixSizeRows * comparingBoard.MatrixSizeColumns) / 2;

            for (int row = 0; row < comparingBoard.MatrixSizeRows; row++)
            {
                for (int col = 0; col < comparingBoard.MatrixSizeColumns; col++)
                {
                    if (comparingBoard.Matrix[row,col] != engineBoard.Matrix[row,col])
                    {
                        differences++;
                    }
                }
            }

            bool areEqual = true;
            if (differences > MinCountOfDifferences)
            {
                areEqual = false;
            }

            Assert.IsFalse(areEqual);
        }
    }
}
