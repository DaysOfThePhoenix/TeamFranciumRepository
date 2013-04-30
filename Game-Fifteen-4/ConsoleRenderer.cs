using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFifteen
{
    public static class ConsoleRenderer
    {
        public static void RenderMessage(string message)
        {
            Console.WriteLine(message);
        }

        internal static void RenderMatrix()
        {
            StringBuilder horizontalBorder = new StringBuilder("  ");

            for (int i = 0; i < MainMethod.MatrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- ");
            Console.WriteLine(horizontalBorder);

            for (int row = 0; row < MainMethod.MatrixSizeRows; row++)
            {
                Console.Write(" |");

                for (int column = 0; column < MainMethod.MatrixSizeColumns; column++)
                {
                    Console.Write("{0,3}", Board.matrix[row, column]);
                }

                Console.WriteLine(" |");
            }

            Console.WriteLine(horizontalBorder);
        }
    }
}
