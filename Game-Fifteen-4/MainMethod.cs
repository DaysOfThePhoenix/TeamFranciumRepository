using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameFifteen
{
    class MainMethod
    {
        private const string EmptyCellValue = " ";
        private const int MatrixSizeRows = 4;
        private const int MatrixSizeColumns = 4;

        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
        private static int emptyCellRow;
        private static int emptyCellColumn;
        private static string[,] matrix;
        private static Random random = new Random();
        internal static int turn;

        private static int CellNumberToDirection(int cellNumber)
        {
            int direction = -1;

            for (int dir = 0; dir < DirectionRow.Length; dir++)
            {
                bool isDirValid = proverka(dir);

                if (isDirValid)
                {    
                    int nextCellRow = emptyCellRow + DirectionRow[dir];   
                    int nextCellColumn = emptyCellColumn + DirectionColumn[dir];

                    if (matrix[nextCellRow,nextCellColumn] == cellNumber.ToString())
                    {
                        direction = dir;
                        break;
                    }
                }
            }

            return direction;
        }

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

        private static void InitializeMatrix()
        {
            matrix = new string[MatrixSizeRows,MatrixSizeColumns];
            int cellValue = 1;

            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns; column++)   
                {
                    matrix[row,column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = MatrixSizeRows - 1;
            emptyCellColumn = MatrixSizeColumns - 1;
            matrix[emptyCellRow,emptyCellColumn] = EmptyCellValue;
        }

        private static bool proverka(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];   
            bool isRowValid = (nextCellRow >= 0 && nextCellRow < MatrixSizeRows);
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns);
            bool isCellValid = isRowValid && isColumnValid;
            
            return isCellValid;
        }

        private static bool proverka2()
        {
            bool isEmptyCellInPlace = emptyCellRow == MatrixSizeRows - 1 && 
                emptyCellColumn == MatrixSizeColumns - 1;

            if (!isEmptyCellInPlace)
            {
                return false;
            }

            int cellValue = 1;
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            
            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns && cellValue < matrixSize; column++)   
                {
                    if (matrix[row,column] != cellValue.ToString())   
                    {
                        return false;
                    }

                    cellValue++;
                }
            }
            
            return true;
        }

        private static void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];

            matrix[emptyCellRow,emptyCellColumn] = matrix[nextCellRow,nextCellColumn];
            matrix[nextCellRow,nextCellColumn] = EmptyCellValue;

            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;

            turn++;
        }

        private static void NextMove(int cellNumber)
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;

            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                Messages.PrintCellDoesNotExistMessage();
                return;
            }

            int direction = CellNumberToDirection(cellNumber);

            if (direction == -1)
            {
                Messages.PrintIllegalMoveMessage();
                return;
            }

            MoveCell(direction);
            PrintMatrix();
        }

        public static void PlayGame()
        {
            while (true)
            {
                InitializeMatrix();
                ShuffleMatrix();
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
                        NextMove(cellNumber);
                        if (proverka2())
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
                                PrintTopScores();
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

        private static void PrintMatrix()
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
                    Console.Write("{0,3}", matrix[row,column]);
                }

                Console.WriteLine(" |");
            }

            Console.WriteLine(horizontalBorder);
        }

        private static void PrintTopScores()
        {
            Console.WriteLine("Scoreboard:");
            string[] topScores = Score.GetTopScoresFromFile();

            if (topScores[0] == null)
            {
                Console.WriteLine("There are no scores to display yet.");
            }
            else
            {
                foreach (string score in topScores)
                {
                    if (score != null)
                    {
                        Console.WriteLine(score);
                    }
                }
            }

        }

        private static void ShuffleMatrix()
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            int shuffles = random.Next(matrixSize, matrixSize * 100);

            for (int i = 0; i < shuffles; i++)
            {
                int direction = random.Next(DirectionRow.Length);
                if (proverka(direction))
                {
                    MoveCell(direction);
                }
            }

            if (proverka2())
            {
                ShuffleMatrix();
            }
        }

        private static Score[] UpgradeTopScorePairs(string[] topScores)
        {
            int startIndex = 0;

            while (topScores[startIndex] == null)
            {
                startIndex++;
            }

            int arraySize = Math.Min(Score.TopScoresAmount - startIndex + 1, Score.TopScoresAmount);
            Score[] topScoresPairs = new Score[arraySize];

            for (int topScoresPairsIndex = 0; topScoresPairsIndex < arraySize; topScoresPairsIndex++)
            {
                int topScoresIndex = topScoresPairsIndex + startIndex;
                string name = Regex.Replace(topScores[topScoresIndex], Score.TopScoresPersonPattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], Score.TopScoresPersonPattern, @"$2");
                int scoreInt = int.Parse(score);
                topScoresPairs[topScoresPairsIndex] = new Score(name, scoreInt);
            }

            return topScoresPairs;
        }

        static void Main()
        {
            PlayGame();
        }
    }
}
