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

        internal string GetHorizontalBorder()
        {
            StringBuilder horizontalBorder = new StringBuilder();

            horizontalBorder.Append("  ");

            for (int i = 0; i < Board.MatrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- ");

            return horizontalBorder.ToString();
        }

        internal void RenderMatrix()
        {
            StringBuilder matrixToString = new StringBuilder();

            string horizontalBorder = GetHorizontalBorder();

            matrixToString.AppendLine(horizontalBorder);

            for (int row = 0; row < Board.MatrixSizeRows; row++)
            {
                matrixToString.Append(" |");

                for (int column = 0; column < Board.MatrixSizeColumns; column++)
                {
                    matrixToString.AppendFormat("{0,3}", Board.Matrix[row, column]);
                }

                matrixToString.AppendLine(" |");
            }

            matrixToString.Append(horizontalBorder);

            Console.WriteLine(matrixToString);
        }
    }
}
