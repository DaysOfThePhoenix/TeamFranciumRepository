using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameFifteen
{
    class MainMethod
    {
        private const int MatrixSizeRows = 4;
        private const int MatrixSizeColumns = 4;
        private static string[,] matrix;

        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
        private static int emptyCellRow;
        private static int emptyCellColumn;
        private static Random random = new Random();
        internal static int turn;

        private static void TheEnd()
        {
            string moves = turn == 1 ? "1 move" : string.Format("{0} moves", turn);
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
            string[] topScores = Score.GetTopScoresFromFile();

            if (topScores[Score.TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[Score.TopScoresAmount - 1], Score.TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < turn)
                {
                    Console.WriteLine("You couldn't get in the top {0} scoreboard.", Score.TopScoresAmount);
                    return;
                }
            }

            Score.UpgradeTopScore();
        }

        public static void PlayGame()
        {
            while (true)
            {
                Board.InitializeMatrix();
                Board.ShuffleMatrix();
                turn = 0;
                Messages.PrintWelcomeMessage();
                PrintMatrix();
                while (true)
                {
                    Messages.PrintNextMoveMessage();
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        //Input is a cell number.
                        Board.NextMove(cellNumber);
                        if (Board.CheckIfMatrixIsOrderedCorrectly())
                        {
                            TheEnd();
                            break;
                        }
                    }
                    else
                    {
                        //Input is a command.
                        if (consoleInputLine == "restart")
                        {
                            break;
                        }
                        switch (consoleInputLine)
                        {
                            case "top":
                                Score.PrintTopScores();
                                break;
                            case "exit":
                                Messages.PrintGoodbye();
                                return;
                            default:
                                Messages.PrintIllegalCommandMessage();
                                break;
                        }
                    }

                }
            }
        }

        internal static void PrintMatrix()
        {
            StringBuilder horizontalBorder = new StringBuilder("  ");

            for (int i = 0; i < MatrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- ");
            Console.WriteLine(horizontalBorder);

            for (int row = 0; row < MatrixSizeRows; row++)
            {
                Console.Write(" |");

                for (int column = 0; column < MatrixSizeColumns; column++)
                {
                    Console.Write("{0,3}", Board.matrix[row,column]);
                }

                Console.WriteLine(" |");
            }

            Console.WriteLine(horizontalBorder);
        }

        static void Main()
        {
            PlayGame();
        }
    }
}
