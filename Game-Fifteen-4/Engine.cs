﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameFifteen
{
    class Engine
    {
        internal static int turn;

        public static void PlayGame()
        {
            while (true)
            {
                Board.InitializeMatrix();
                Board.ShuffleMatrix();
                turn = 0;
                ConsoleRenderer.RenderMessage(Messages.GetWelcomeMessage());
                ConsoleRenderer.RenderMatrix();
                while (true)
                {
                    ConsoleRenderer.RenderMessage(Messages.GetNextMoveMessage());
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;

                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        //Input is a cell number.
                        Board.NextMove(cellNumber);
                        if (Board.CheckIfMatrixIsOrderedCorrectly())
                        {
                            TheEnd();
                            break;
                        }
                    }
                    else
                    {
                        //Input is a command.
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
                                ConsoleRenderer.RenderMessage(Messages.GetGoodbye());
                                return;
                            default:
                                ConsoleRenderer.RenderMessage(Messages.GetIllegalCommandMessage());
                                break;
                        }
                    }

                }
            }
        }

        private static void TheEnd()
        {
            string moves = turn == 1 ? "1 move" : string.Format("{0} moves", turn);
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
            string[] topScores = Score.GetTopScoresFromFile();

            if (topScores[Score.TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[Score.TopScoresAmount - 1], Score.TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < turn)
                {
                    Console.WriteLine("You couldn't get in the top {0} scoreboard.", Score.TopScoresAmount);
                    return;
                }
            }

            Score.UpgradeTopScore();
        }
    }
}
