﻿namespace GameFifteen
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Score
    {
        private string name;
        private int points;
        private int topScoresCount;
        private string fileNameForExternalSave;
        public string TopScoresPersonPattern {get; private set;}

        public Score(string name, int score, int topScoresCount, string fileNameForExternalSave)
        {
            this.name = name;
            this.points = score;
            this.topScoresCount = topScoresCount;
            this.fileNameForExternalSave = fileNameForExternalSave;
            this.TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public int Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
            }
        }

        public int TopScoresCount
        {
            get
            {
                return this.topScoresCount;
            }

            set
            {
                this.topScoresCount = value;
            }
        }

        public string FileNameForExternalSave
        {
            get
            {
                return this.fileNameForExternalSave;
            }

            set
            {
                this.fileNameForExternalSave = value;
            }
        }

        internal string[] GetTopScoresFromFile()
        {
            try
            {
                string[] topScores = new string[this.TopScoresCount + 1];
                StreamReader topReader = new StreamReader(this.FileNameForExternalSave);

                using (topReader)
                {
                    int line = 0;

                    while (!topReader.EndOfStream && line < this.TopScoresCount)
                    {
                        topScores[line] = topReader.ReadLine();
                        line++;
                    }
                }

                return topScores;
            }
            catch (FileNotFoundException)
            {
                StreamWriter topWriter = new StreamWriter(this.FileNameForExternalSave);

                using (topWriter)
                {
                    topWriter.Write(string.Empty);
                }

                return new string[this.TopScoresCount];
            }
        }

        internal void UpgradeTopScoreInFile(IOrderedEnumerable<Score> sortedScores)
        {
            StreamWriter topWriter = new StreamWriter(this.FileNameForExternalSave);

            using (topWriter)
            {
                int position = 1;

                foreach (Score pair in sortedScores)
                {
                    string name = pair.Name;
                    int score = pair.Points;
                    string toWrite = string.Format("{0}. {1} --> {2} move", position, name, score);

                    if (score > 1)
                    {
                        toWrite += "s";
                    }

                    topWriter.WriteLine(toWrite);
                    position++;
                }
            }
        }

        internal void UpgradeTopScore()
        {
            string[] topScores = GetTopScoresFromFile();
            Console.Write("Please enter your name for the top scoreboard: ");
            string name = Console.ReadLine();

            if (name == string.Empty)
            {
                name = "Anonymous";
            }

            topScores[this.TopScoresCount] = string.Format("0. {0} --> {1} move", name, Turn.Count);

            Array.Sort(topScores);
            Score[] topScoresPairs = UpgradeTopScorePairs(topScores);
            IOrderedEnumerable<Score> sortedScores =
            topScoresPairs.OrderBy(x => x.Points).ThenBy(x => x.Name);
            this.UpgradeTopScoreInFile(sortedScores);
        }

        internal void PrintTopScores()
        {
            Console.WriteLine("Scoreboard:");
            string[] topScores = this.GetTopScoresFromFile();

            if (topScores[0] == null)
            {
                Console.WriteLine("There are no scores to display yet.");
            }
            else
            {
                foreach (string score in topScores)
                {
                    if (score != null)
                    {
                        Console.WriteLine(score);
                    }
                }
            }
        }

        internal Score[] UpgradeTopScorePairs(string[] topScores)
        {
            int startIndex = 0;

            while (topScores[startIndex] == null)
            {
                startIndex++;
            }

            int arraySize = Math.Min(this.TopScoresCount - startIndex + 1, this.TopScoresCount);
            Score[] topScoresPairs = new Score[arraySize];

            for (int topScoresPairsIndex = 0; topScoresPairsIndex < arraySize; topScoresPairsIndex++)
            {
                int topScoresIndex = topScoresPairsIndex + startIndex;
                string name = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$2");
                int scoreInt = int.Parse(score);
                topScoresPairs[topScoresPairsIndex] = new Score(name, scoreInt, this.TopScoresCount, this.FileNameForExternalSave);
            }

            return topScoresPairs;
        }
    }
}