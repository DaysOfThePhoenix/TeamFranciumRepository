namespace GameFifteen
{
    using System;
    using System.Text;

    public static class Messages
    {
        internal static string GetCellDoesNotExistMessage()
        {
            return "That cell does not exist in the matrix.";
        }

        internal static string GetGoodbye()
        {
            return "Good bye!";
        }

        internal static string GetIllegalCommandMessage()
        {
            return "Illegal command!";
        }

        internal static string GetIllegalMoveMessage()
        {
            return "Illegal move!";
        }

        internal static string GetNextMoveMessage()
        {
            return "Enter a number to move: ";
        }

        internal static string GetWelcomeMessage()
        {
            StringBuilder welcomeMessage = new StringBuilder();

            welcomeMessage.AppendLine("Welcome to the Game \"15\". ");
            welcomeMessage.AppendLine("Please try to arrange the numbers sequentially. ");
            welcomeMessage.AppendLine("Menu:");
            welcomeMessage.AppendLine("top - view the top scoreboard");
            welcomeMessage.AppendLine("restart - start a new game");
            welcomeMessage.Append("exit - quit the game");

            return welcomeMessage.ToString();
        }
    }
}
