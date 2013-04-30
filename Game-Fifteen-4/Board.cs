namespace GameFifteen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Board
    {
        private const string EmptyCellValue = " ";
        internal const int MatrixSizeRows = 4;
        internal const int MatrixSizeColumns = 4;
        internal static string[,] matrix;

        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
        private static int emptyCellRow;
        private static int emptyCellColumn;

        private static Random random = new Random();

        internal static void InitializeMatrix()
        {
            matrix = new string[MatrixSizeRows, MatrixSizeColumns];
            int cellValue = 1;

            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns; column++)
                {
                    matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = MatrixSizeRows - 1;
            emptyCellColumn = MatrixSizeColumns - 1;
            matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
        }

        internal static int CellNumberToDirection(int cellNumber)
        {
            int direction = -1;

            for (int dir = 0; dir < DirectionRow.Length; dir++)
            {
                bool isDirValid = ValidateNextCell(dir);

                if (isDirValid)
                {
                    int nextCellRow = emptyCellRow + DirectionRow[dir];
                    int nextCellColumn = emptyCellColumn + DirectionColumn[dir];

                    if (matrix[nextCellRow, nextCellColumn] == cellNumber.ToString())
                    {
                        direction = dir;
                        break;
                    }
                }
            }

            return direction;
        }

        internal static bool ValidateNextCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            bool isRowValid = (nextCellRow >= 0 && nextCellRow < MatrixSizeRows);
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns);
            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        internal static void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];

            matrix[emptyCellRow, emptyCellColumn] = matrix[nextCellRow, nextCellColumn];
            matrix[nextCellRow, nextCellColumn] = EmptyCellValue;

            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;

            Engine.turn++;
        }

        internal static bool CheckIfMatrixIsOrderedCorrectly()
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
                    if (matrix[row, column] != cellValue.ToString())
                    {
                        return false;
                    }

                    cellValue++;
                }
            }

            return true;
        }

        internal static void ShuffleMatrix()
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            int shuffles = random.Next(matrixSize, matrixSize * 100);

            for (int i = 0; i < shuffles; i++)
            {
                int direction = random.Next(DirectionRow.Length);
                if (Board.ValidateNextCell(direction))
                {
                    Board.MoveCell(direction);
                }
            }

            if (Board.CheckIfMatrixIsOrderedCorrectly())
            {
                ShuffleMatrix();
            }

        }

        internal static void NextMove(int cellNumber)
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;

            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                ConsoleRenderer.RenderMessage(Messages.GetCellDoesNotExistMessage());
                return;
            }

            int direction = Board.CellNumberToDirection(cellNumber);

            if (direction == -1)
            {
                ConsoleRenderer.RenderMessage(Messages.GetIllegalMoveMessage());
                return;
            }

            Board.MoveCell(direction);
            ConsoleRenderer.RenderMatrix();
        }
    }
}
