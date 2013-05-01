namespace GameFifteen
{
    using System;
    using System.Text.RegularExpressions;

    internal class Engine
    {
        private ConsoleRenderer renderer;
        private Board board;
        private Score score;

        public Engine( ConsoleRenderer renderer, Board board, Score score)
        {
            this.renderer = renderer;
            this.board = board;
            this.score = score;
        }

        public ConsoleRenderer Renderer
        {
            get 
            {
                return this.renderer;
            }

            private set
            {
                this.renderer = value;
            }
        }

        public Board Board
        {
            get
            {
                return this.board;
            }

            private set
            {
                this.board = value;
            }
        }

        public Score Score
        {
            get
            {
                return this.score;
            }

            private set
            {
                this.score = value;
            }
        }

        public void PlayGame()
        {
            while (true)
            {
                ConsoleRenderer renderer = new ConsoleRenderer();
                Board board = new Board(4,4);
                Score score = new Score("Francium", 0, 5, "top.txt");

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
                        NextMove(cellNumber, board, renderer);
                        if (board.CheckIfMatrixIsOrderedCorrectly())
                        {
                            GameOver(score);
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
                                score.PrintTopScores();
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

        internal void NextMove(int cellNumber, Board board, ConsoleRenderer renderer)
        {
            int matrixSize = board.MatrixSizeRows * board.MatrixSizeColumns;

            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                renderer.RenderMessage(Messages.GetCellDoesNotExistMessage());
                return;
            }

            int direction = board.CellNumberToDirection(cellNumber);

            if (direction == -1)
            {
                renderer.RenderMessage(Messages.GetIllegalMoveMessage());
                return;
            }

            board.MoveCell(direction);
            renderer.RenderMatrix(board);
        }

        private void GameOver(Score score)
        {
            string moves = Turn.Count == 1 ? "1 move" : string.Format("{0} moves", Turn.Count);
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
            string[] topScores = score.GetTopScoresFromFile();

            if (topScores[score.TopScoresCount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[score.TopScoresCount - 1], score.TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < Turn.Count)
                {
                    Console.WriteLine("You couldn't get in the top {0} scoreboard.", score.TopScoresCount);
                    return;
                }
            }

            score.UpgradeTopScore();
        }
    }
}
