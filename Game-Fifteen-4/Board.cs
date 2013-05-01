﻿namespace GameFifteen
{
    using System;

    internal class Board
    {
        internal const int MatrixSizeRows = 4;
        internal const int MatrixSizeColumns = 4;

        internal string[,] Matrix; //documnt rename

        private const string EmptyCellValue = " ";
        private static readonly int[] DirectionRow = { -1, 0, 1, 0 }; // documnt rename
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 }; // documnt rename
        private static readonly Random Random = new Random();
        private int emptyCellRow;
        private int emptyCellColumn;

        public Board()
        {
            this.InitializeMatrix();
        }

        internal void InitializeMatrix()
        {
            this.Matrix = new string[Board.MatrixSizeRows, Board.MatrixSizeColumns];
            int cellValue = 1;

            for (int row = 0; row < Board.MatrixSizeRows; row++)
            {
                for (int column = 0; column < Board.MatrixSizeColumns; column++)
                {
                    this.Matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            this.emptyCellRow = MatrixSizeRows - 1;
            this.emptyCellColumn = MatrixSizeColumns - 1;
            this.Matrix[emptyCellRow, emptyCellColumn] = Board.EmptyCellValue;
        }

        internal int CellNumberToDirection(int cellNumber)
        {
            int direction = -1;

            for (int dir = 0; dir < DirectionRow.Length; dir++)
            {
                bool isDirValid = ValidateNextCell(dir);

                if (isDirValid)
                {
                    int nextCellRow = this.emptyCellRow + Board.DirectionRow[dir];
                    int nextCellColumn = this.emptyCellColumn + Board.DirectionColumn[dir];

                    if (this.Matrix[nextCellRow, nextCellColumn] == cellNumber.ToString())
                    {
                        direction = dir;
                        break;
                    }
                }
            }

            return direction;
        }

        internal bool ValidateNextCell(int direction)
        {
            int nextCellRow = this.emptyCellRow + Board.DirectionRow[direction];
            bool isRowValid = (nextCellRow >= 0) && (nextCellRow < Board.MatrixSizeRows);

            int nextCellColumn = this.emptyCellColumn + Board.DirectionColumn[direction];
            bool isColumnValid = (nextCellColumn >= 0) && (nextCellColumn < Board.MatrixSizeColumns);

            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        internal void MoveCell(int direction)
        {
            int nextCellRow = this.emptyCellRow + DirectionRow[direction];
            int nextCellColumn = this.emptyCellColumn + DirectionColumn[direction];

            this.Matrix[this.emptyCellRow, this.emptyCellColumn] = this.Matrix[nextCellRow, nextCellColumn];
            Matrix[nextCellRow, nextCellColumn] = EmptyCellValue;

            this.emptyCellRow = nextCellRow;
            this.emptyCellColumn = nextCellColumn;

            CurrentTurn.Turn++;
        }

        internal bool CheckIfMatrixIsOrderedCorrectly()
        {
            bool isEmptyCellInPlace = this.emptyCellRow == Board.MatrixSizeRows - 1 &&
                this.emptyCellColumn == Board.MatrixSizeColumns - 1;

            if (!isEmptyCellInPlace)
            {
                return false;
            }

            int cellValue = 1;
            int matrixSize = Board.MatrixSizeRows * Board.MatrixSizeColumns;

            for (int row = 0; row < Board.MatrixSizeRows; row++)
            {
                for (int column = 0; column < Board.MatrixSizeColumns && cellValue < matrixSize; column++)
                {
                    if (this.Matrix[row, column] != cellValue.ToString())
                    {
                        return false;
                    }

                    cellValue++;
                }
            }

            return true;
        }

        internal void ShuffleMatrix()
        {
            int matrixSize = Board.MatrixSizeRows * Board.MatrixSizeColumns;
            int shuffles = Random.Next(matrixSize, matrixSize * 100);

            for (int i = 0; i < shuffles; i++)
            {
                int direction = Random.Next(DirectionRow.Length);
                if (this.ValidateNextCell(direction))
                {
                    this.MoveCell(direction);
                }
            }

            if (this.CheckIfMatrixIsOrderedCorrectly())
            {
                this.ShuffleMatrix();
            }
        }

        internal void NextMove(int cellNumber)
        {
            int matrixSize = Board.MatrixSizeRows * Board.MatrixSizeColumns;

            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                renderer.RenderMessage(Messages.GetCellDoesNotExistMessage());
                return;
            }

            int direction = this.CellNumberToDirection(cellNumber);

            if (direction == -1)
            {
                renderer.RenderMessage(Messages.GetIllegalMoveMessage());
                return;
            }

            this.MoveCell(direction);
            renderer.RenderMatrix(this);
        }
    }
}
