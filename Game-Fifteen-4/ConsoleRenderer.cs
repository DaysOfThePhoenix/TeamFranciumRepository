namespace GameFifteen
{
    using System;
    using System.Text;

    internal class ConsoleRenderer
    {
        internal ConsoleRenderer()
        {
        }

        internal void RenderMessage(string message)
        {
            Console.WriteLine(message);
        }

        private string GetHorizontalBorder(Board gameField)
        {
            StringBuilder horizontalBorder = new StringBuilder();

            horizontalBorder.Append("  ");

            for (int i = 0; i < gameField.MatrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- ");

            return horizontalBorder.ToString();
        }

        internal void RenderMatrix(Board gameField)
        {
            StringBuilder matrixToString = new StringBuilder();

            string horizontalBorder = GetHorizontalBorder(gameField);

            matrixToString.AppendLine(horizontalBorder);

            for (int row = 0; row < gameField.MatrixSizeRows; row++)
            {
                matrixToString.Append(" |");

                for (int column = 0; column < gameField.MatrixSizeColumns; column++)
                {
                    matrixToString.AppendFormat("{0,3}", gameField.Matrix[row, column]);
                }

                matrixToString.AppendLine(" |");
            }

            matrixToString.Append(horizontalBorder);

            Console.WriteLine(matrixToString);
        }
    }
}
