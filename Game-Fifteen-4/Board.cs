namespace GameFifteen
{
    using System;

    internal class Board
    {
        private const string EmptyCellValue = " ";
        internal const int MatrixSizeRows = 4;
        internal const int MatrixSizeColumns = 4;
        internal static string[,] Matrix; //documnt rename

        private static readonly int[] directionRow = { -1, 0, 1, 0 };//documnt rename
        private static readonly int[] directionColumn = { 0, 1, 0, -1 };//documnt rename
        private static int emptyCellRow;
        private static int emptyCellColumn;

        private static readonly Random random = new Random();

        internal static void InitializeMatrix()
        {
            Matrix = new string[MatrixSizeRows, MatrixSizeColumns];
            int cellValue = 1;

            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns; column++)
                {
                    Matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = MatrixSizeRows - 1;
            emptyCellColumn = MatrixSizeColumns - 1;
            Matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
        }

        internal static int CellNumberToDirection(int cellNumber)
        {
            int direction = -1;

            for (int dir = 0; dir < directionRow.Length; dir++)
            {
                bool isDirValid = ValidateNextCell(dir);

                if (isDirValid)
                {
                    int nextCellRow = emptyCellRow + directionRow[dir];
                    int nextCellColumn = emptyCellColumn + directionColumn[dir];

                    if (Matrix[nextCellRow, nextCellColumn] == cellNumber.ToString())
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
            int nextCellRow = emptyCellRow + directionRow[direction];
            bool isRowValid = (nextCellRow >= 0 && nextCellRow < MatrixSizeRows);
            int nextCellColumn = emptyCellColumn + directionColumn[direction];
            bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns);
            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        internal static void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + directionRow[direction];
            int nextCellColumn = emptyCellColumn + directionColumn[direction];

            Matrix[emptyCellRow, emptyCellColumn] = Matrix[nextCellRow, nextCellColumn];
            Matrix[nextCellRow, nextCellColumn] = EmptyCellValue;

            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;

            CurrentTurn.Turn++;
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
                    if (Matrix[row, column] != cellValue.ToString())
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
                int direction = random.Next(directionRow.Length);
                if (ValidateNextCell(direction))
                {
                    MoveCell(direction);
                }
            }

            if (CheckIfMatrixIsOrderedCorrectly())
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

            MoveCell(direction);
            ConsoleRenderer.RenderMatrix();
        }
    }
}
