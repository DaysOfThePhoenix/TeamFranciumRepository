namespace GameFifteen
{
    struct Scores
    {
        private string name;
        private int score;

        public Scores(string name, int score)
        {
            this.name = name;
            this.score = score;
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

        public int Score
        {
            get
            {
            
                return score;
            }
   
            set
            {
                score = value;
            }
        }
    }
}




