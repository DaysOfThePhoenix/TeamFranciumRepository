using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFifteen
{
    internal static class ConsoleRenderer
    {
        public static void RenderMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static string GetHorizontalBorder()
        {
            StringBuilder horizontalBorder = new StringBuilder();

            for (int i = 0; i < Board.MatrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- ");

            return horizontalBorder.ToString();
        }

        internal static void RenderMatrix()
        {
            StringBuilder matrixToString = new StringBuilder("  ");

            string horizontalBorder = GetHorizontalBorder();

            matrixToString.AppendLine(horizontalBorder);

            for (int row = 0; row < Board.MatrixSizeRows; row++)
            {
                matrixToString.Append(" |");

                for (int column = 0; column < Board.MatrixSizeColumns; column++)
                {
                    matrixToString.AppendFormat("{0,3}", Board.matrix[row, column]);
                }

                matrixToString.AppendLine(" |");
            }

            matrixToString.Append(horizontalBorder);

            Console.WriteLine(matrixToString);
        }
    }
}
