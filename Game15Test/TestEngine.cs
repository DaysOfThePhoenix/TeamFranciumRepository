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
        public void TestPlayGameWithCommandExitAndSimpleGameplay()
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
            expectedOutput.AppendLine("Please enter your name for the top scoreboard: ");
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

        [TestMethod]
        public void TestPlayGameWithCommandTop()
        {
            ConsoleRenderer renderer = new ConsoleRenderer();
            Board gameBoard = new Board(3, 3);
            Score playerScore = new Score("Anonymous", 0, 5, "top.txt");
            gameBoard.InitializeMatrix();
            Engine engine = new Engine(renderer, gameBoard, playerScore);

            StringReader input = new StringReader("top\nexit");
            StringWriter output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            engine.PlayGame();

            StringWriter expectedTopScores = new StringWriter();
            Console.SetOut(expectedTopScores);
            renderer.RenderTopScores(playerScore);

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
            expectedOutput.Append(expectedTopScores.ToString());
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("Good bye!");

            Assert.AreEqual(expectedOutput.ToString(), output.ToString());
        }

        [TestMethod]
        public void TestPlayGameWithAnIllegalMove()
        {
            ConsoleRenderer renderer = new ConsoleRenderer();
            Score playerScore = new Score("Anonymous", 0, 5, "top.txt");
            Board gameBoard = new Board(3, 3);
            gameBoard.InitializeMatrix();
            gameBoard.Matrix[0, 0] = "2";
            gameBoard.Matrix[0, 1] = "1";
            Engine engine = new Engine(renderer, gameBoard, playerScore);

            StringReader input = new StringReader("1\nexit");
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
            expectedOutput.AppendLine(" |  2  1  3 |");
            expectedOutput.AppendLine(" |  4  5  6 |");
            expectedOutput.AppendLine(" |  7  8    |");
            expectedOutput.AppendLine("  ---------- ");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("Illegal move!");
            expectedOutput.AppendLine("Enter a number to move: ");
            expectedOutput.AppendLine("Good bye!");

            Assert.AreEqual(expectedOutput.ToString(), output.ToString());
        }
    }
}
