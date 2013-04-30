namespace GameFifteen
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Score
    {
        private string name;
        private int points;

        public Score(string name, int score)
        {
            this.name = name;
            this.points = score;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Points
        {
            get
            {

                return points;
            }

            set
            {
                points = value;
            }
        }

        internal const int TopScoresAmount = 5;
        internal const string TopScoresFileName = "Top.txt";
        internal const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        internal static string[] GetTopScoresFromFile()
        {
            try
            {
                string[] topScores = new string[TopScoresAmount + 1];
                StreamReader topReader = new StreamReader(TopScoresFileName);

                using (topReader)
                {
                    int line = 0;

                    while (!topReader.EndOfStream && line < TopScoresAmount)
                    {
                        topScores[line] = topReader.ReadLine();
                        line++;
                    }

                }

                return topScores;
            }

            catch (FileNotFoundException)
            {
                StreamWriter topWriter = new StreamWriter(TopScoresFileName);

                using (topWriter)
                {
                    topWriter.Write("");
                }

                return new string[TopScoresAmount];
            }

        }

        internal static void UpgradeTopScoreInFile(IOrderedEnumerable<Score> sortedScores)
        {
            StreamWriter topWriter = new StreamWriter(TopScoresFileName);

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

        internal static void UpgradeTopScore()
        {
            string[] topScores = GetTopScoresFromFile();
            Console.Write("Please enter your name for the top scoreboard: ");
            string name = Console.ReadLine();

            if (name == string.Empty)
            {
                name = "Anonymous";
            }

            topScores[TopScoresAmount] = string.Format("0. {0} --> {1} move", name, Engine.turn);

            Array.Sort(topScores);
            Score[] topScoresPairs = UpgradeTopScorePairs(topScores);
            IOrderedEnumerable<Score> sortedScores =
            topScoresPairs.OrderBy(x => x.Points).ThenBy(x => x.Name);
            Score.UpgradeTopScoreInFile(sortedScores);
        }

        internal static void PrintTopScores()
        {
            Console.WriteLine("Scoreboard:");
            string[] topScores = GetTopScoresFromFile();

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

        private static Score[] UpgradeTopScorePairs(string[] topScores)
        {
            int startIndex = 0;

            while (topScores[startIndex] == null)
            {
                startIndex++;
            }

            int arraySize = Math.Min(TopScoresAmount - startIndex + 1, TopScoresAmount);
            Score[] topScoresPairs = new Score[arraySize];

            for (int topScoresPairsIndex = 0; topScoresPairsIndex < arraySize; topScoresPairsIndex++)
            {
                int topScoresIndex = topScoresPairsIndex + startIndex;
                string name = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$2");
                int scoreInt = int.Parse(score);
                topScoresPairs[topScoresPairsIndex] = new Score(name, scoreInt);
            }

            return topScoresPairs;
        }
    }
}




