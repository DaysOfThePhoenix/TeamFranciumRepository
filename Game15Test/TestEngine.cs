using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameFifteen;
using System.IO;
using System.Text;

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
                    if (comparingBoard.Matrix[row, col] != engineBoard.Matrix[row, col])
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

        [TestMethod]
        public void TestPlayGame()
        {
            ConsoleRenderer renderer = new ConsoleRenderer();
            Board gameBoard = new Board(3, 3);
            Score playerScore = new Score("Anonymous", 0, 5, "top.txt");
            gameBoard.InitializeMatrix();
            Engine engine = new Engine(renderer, gameBoard, playerScore);

            StringReader input = new StringReader("6\n6\nrado\nexit");
            StringWriter output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            engine.PlayGame();

            StringBuilder expectedOutput = new StringBuilder();
            expectedOutput.AppendLine("Welcome to the Game \"15\".");
            expectedOutput.AppendLine("Please try to arrange the numbers sequentially.");
            expectedOutput.AppendLine("Menu:");
            expectedOutput.AppendLine("top - view the top scoreboard");
            expectedOutput.AppendLine("restart - start a new game");
            expectedOutput.AppendLine("exit - quit the game");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine(" |  1  2  3 |");
            expectedOutput.AppendLine(" |  4  5  6 |");
            expectedOutput.AppendLine(" |  7  8    |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine(" |  1  2  3 |");
            expectedOutput.AppendLine(" |  4  5    |");
            expectedOutput.AppendLine(" |  7  8  6 |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine(" |  1  2  3 |");
            expectedOutput.AppendLine(" |  4  5  6 |");
            expectedOutput.AppendLine(" |  7  8    |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Congratulations! You won the game in 2 moves.");
            expectedOutput.Append("Please enter your name for the top scoreboard: ");
            expectedOutput.AppendLine("Welcome to the Game \"15\".");
            expectedOutput.AppendLine("Please try to arrange the numbers sequentially.");
            expectedOutput.AppendLine("Menu:");
            expectedOutput.AppendLine("top - view the top scoreboard");
            expectedOutput.AppendLine("restart - start a new game");
            expectedOutput.AppendLine("exit - quit the game");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine(" |  1  2  3 |");
            expectedOutput.AppendLine(" |  4  5  6 |");
            expectedOutput.AppendLine(" |  7  8    |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("Good bye!");

            Assert.AreEqual(expectedOutput.ToString(), output.ToString());
        }

        [TestMethod]
        public void TestPlayGameWithCommandRestart()
        {
            ConsoleRenderer renderer = new ConsoleRenderer();
            Board gameBoard = new Board(3, 3);
            Score playerScore = new Score("Anonymous", 0, 5, "top.txt");
            gameBoard.InitializeMatrix();
            Engine engine = new Engine(renderer, gameBoard, playerScore);

            StringReader input = new StringReader("restart\nexit");
            StringWriter output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            engine.PlayGame();

            StringWriter matrixAfterRestartCommand = new StringWriter();
            Console.SetOut(matrixAfterRestartCommand);
            renderer.RenderMatrix(engine.Board);

            StringBuilder expectedOutput = new StringBuilder();
            expectedOutput.AppendLine("Welcome to the Game \"15\".");
            expectedOutput.AppendLine("Please try to arrange the numbers sequentially.");
            expectedOutput.AppendLine("Menu:");
            expectedOutput.AppendLine("top - view the top scoreboard");
            expectedOutput.AppendLine("restart - start a new game");
            expectedOutput.AppendLine("exit - quit the game");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine(" |  1  2  3 |");
            expectedOutput.AppendLine(" |  4  5  6 |");
            expectedOutput.AppendLine(" |  7  8    |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.Append(matrixAfterRestartCommand.ToString());
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("Good bye!");

            Assert.AreEqual(expectedOutput.ToString(), output.ToString());
        }
    }
}
/*
 Welcome to the Game "15".
Please try to arrange the numbers sequentially.
Menu:
top - view the top scoreboard
restart - start a new game
exit - quit the game
  ----------
 |     1  7 |
 |  5  8  3 |
 |  6  4  2 |
  ----------
Enter a number to move:
restart
  ----------
 |  3     5 |
 |  6  4  2 |
 |  8  7  1 |
  ----------
Enter a number to move:

*/