﻿namespace GameFifteen
{
    using System;
    using System.Text.RegularExpressions;

    internal class Engine
    {
        public ConsoleRenderer Renderer { get; private set; }
        public Board Board { get; private set; }
        public Score Score { get; private set; }

        public Engine( ConsoleRenderer renderer, Board board, Score score)
        {
            this.Renderer = renderer;
            this.Board = board;
            this.Score = score;
        }

        public void PlayGame()
        {
            while (true)
            {
                this.Board.InitializeMatrix();
                this.Board.ShuffleMatrix();

                Turn.Count = 0;

                this.Renderer.RenderMessage(Messages.GetWelcomeMessage());
                this.Renderer.RenderMatrix(Board);

                while (true)
                {
                    this.Renderer.RenderMessage(Messages.GetNextMoveMessage());
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        // Input is a cell number.
                        NextMove(cellNumber, this.Board, this.Renderer);
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
