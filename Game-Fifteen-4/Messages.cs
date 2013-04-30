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

        internal static void PrintWelcomeMessage()
        {
            Console.Write("Welcome to the game \"15\". ");
            Console.WriteLine("Please try to arrange the numbers sequentially. ");
            Console.WriteLine("Use 'top' to view the top scoreboard, " +
                              "'restart' to start a new game and 'exit'  to quit the game.");
        }
    }
}
