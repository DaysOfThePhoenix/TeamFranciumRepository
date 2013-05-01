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
                ConsoleRenderer renderer = new ConsoleRenderer();
                Board board = new Board(4,4);
                Score score = new Score("Francium", 0, 5, "top.txt");

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
                        this.NextMove(cellNumber);
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
            this.Renderer.RenderMatrix(this.Board);
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
