namespace GameFifteen
{
    using System;

    public static class Messages
    {
        internal static void PrintCellDoesNotExistMessage()
        {
            Console.WriteLine("That cell does not exist in the matrix.");
        }

        internal static void PrintGoodbye()
        {
            Console.WriteLine("Good bye!");
        }

        internal static void PrintIllegalCommandMessage()
        {
            Console.WriteLine("Illegal command!");
        }

        internal static void PrintIllegalMoveMessage()
        {
            Console.WriteLine("Illegal move!");
        }

        internal static void PrintNextMoveMessage()
        {
            Console.Write("Enter a number to move: ");
        }
    }
}
