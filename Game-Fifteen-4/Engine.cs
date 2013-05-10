namespace GameFifteen
{
    using System;
    using System.Text.RegularExpressions;

    public class Engine
    {
        private ConsoleRenderer renderer;
        private Board board;
        private Score score;

        public Engine(ConsoleRenderer renderer, Board board, Score score)
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
                Turn.Count = 0;

                this.Renderer.RenderMessage(Messages.GetWelcomeMessage());
                this.Renderer.RenderMessage(this.Renderer.FormatMatrix(Board));

                while (true)
                {
                    this.Renderer.RenderMessage(Messages.GetNextMoveMessage());
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        // Input is a cell number.
                        this.NextMove(cellNumber);
                        if (this.Board.CheckIfMatrixIsOrderedCorrectly())
                        {
                            this.GameOver(Score);
                            break;
                        }
                    }
                    else
                    {
                        // Input is a command.
                        if (consoleInputLine == "restart")
                        {
                            this.board = new Board(4, 4);
                            break;
                        }

                        switch (consoleInputLine)
                        {
                            case "top":
                                this.Score.PrintTopScores();
                                break;
                            case "exit":
                                this.Renderer.RenderMessage(Messages.GetGoodbye());
                                return;
                            default:
                                this.Renderer.RenderMessage(Messages.GetIllegalCommandMessage());
                                break;
                        }
                    }
                }
            }
        }

        private void NextMove(int cellNumber)
        {
            int matrixSize = this.Board.MatrixSizeRows * this.Board.MatrixSizeColumns;

            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                this.Renderer.RenderMessage(Messages.GetCellDoesNotExistMessage());
                return;
            }

            int direction = this.Board.CellNumberToDirection(cellNumber);

            if (direction == -1)
            {
                this.Renderer.RenderMessage(Messages.GetIllegalMoveMessage());
                return;
            }

            this.Board.MoveCell(direction);
            this.Renderer.RenderMessage(this.Renderer.FormatMatrix(this.Board));
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
