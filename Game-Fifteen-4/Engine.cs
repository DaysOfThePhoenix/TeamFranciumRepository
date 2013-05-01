namespace GameFifteen
{
    using System;
    using System.Text.RegularExpressions;

    internal class Engine
    {
        public Engine()
        {
        }

        public void PlayGame()
        {
            while (true)
            {
                ConsoleRenderer renderer = new ConsoleRenderer();
                Board board = new Board();

                board.InitializeMatrix();
                board.ShuffleMatrix();

                Turn.Count = 0;

                renderer.RenderMessage(Messages.GetWelcomeMessage());
                renderer.RenderMatrix(board);

                while (true)
                {
                    renderer.RenderMessage(Messages.GetNextMoveMessage());
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        // Input is a cell number.
                        board.NextMove(cellNumber , renderer);
                        if (board.CheckIfMatrixIsOrderedCorrectly())
                        {
                            TheEnd();
                            break;
                        }
                    }
                    else
                    {
                        // Input is a command.
                        if (consoleInputLine == "restart")
                        {
                            break;
                        }

                        switch (consoleInputLine)
                        {
                            case "top":
                                Score.PrintTopScores();
                                break;
                            case "exit":
                                renderer.RenderMessage(Messages.GetGoodbye());
                                return;
                            default:
                                renderer.RenderMessage(Messages.GetIllegalCommandMessage());
                                break;
                        }
                    }
                }
            }
        }

        private void TheEnd()
        {
            string moves = Turn.Count == 1 ? "1 move" : string.Format("{0} moves", Turn.Count);
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
            string[] topScores = Score.GetTopScoresFromFile();

            if (topScores[Score.TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[Score.TopScoresAmount - 1], Score.TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < Turn.Count)
                {
                    Console.WriteLine("You couldn't get in the top {0} scoreboard.", Score.TopScoresAmount);
                    return;
                }
            }

            Score.UpgradeTopScore();
        }
    }
}
