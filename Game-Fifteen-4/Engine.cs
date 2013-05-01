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

                Board.InitializeMatrix();
                Board.ShuffleMatrix();
                CurrentTurn.Turn = 0;
                renderer.RenderMessage(Messages.GetWelcomeMessage());
                renderer.RenderMatrix();
                while (true)
                {
                    renderer.RenderMessage(Messages.GetNextMoveMessage());
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        // Input is a cell number.
                        Board.NextMove(cellNumber , renderer);
                        if (Board.CheckIfMatrixIsOrderedCorrectly())
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
            string moves = CurrentTurn.Turn == 1 ? "1 move" : string.Format("{0} moves", CurrentTurn.Turn);
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
            string[] topScores = Score.GetTopScoresFromFile();

            if (topScores[Score.TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[Score.TopScoresAmount - 1], Score.TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < CurrentTurn.Turn)
                {
                    Console.WriteLine("You couldn't get in the top {0} scoreboard.", Score.TopScoresAmount);
                    return;
                }
            }

            Score.UpgradeTopScore();
        }
    }
}
