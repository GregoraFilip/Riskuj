using Riskuj.Exception_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Riskuj.GameComponents
{
    public class Question
    {
        private Random rand = new Random();
        public string Text { get; }
        public int Points { get; }
        public bool Answered { get; private set; } = false;
        public IEnumerable<Answer> Answers { get ; }
        private int indexRight;
        private IEnumerable<Answer> shuffledAnswers;
        public Question(int points, string text, IEnumerable<Answer> answers)
        {
            if (answers.Count() == 0)
            {
                Text = text;
                Points = points;
                Answers = new List<Answer>();
                shuffledAnswers = new List<Answer>();
            }
            else if (answers.Count() == 4)
            {
                Text = text;
                Points = points;
                Answers = answers;
                var helper = Answers.Select(a => (a, rand.Next() * rand.Next())).ToList();
                indexRight = helper.Where(i => i.Item2 < helper.First().Item2).Count();


                shuffledAnswers = helper.OrderBy(i => i.Item2).Select(i => i.Item1);
            }
            else
            {
                throw new NotValidGameFile();
            }
        }
        public bool IsBrick()
        {
            return !Answers.Any();
        }
        public IEnumerable<Answer> GetAnswers()
        {
            return shuffledAnswers;
        }
        public int Answer(int index)
        {
            Answered = true;
            if (index != -1)
            {
                shuffledAnswers.ElementAt(index).Chosen = true;
            }

            if (IsBrick())
            {
                return Points == -1 ? 3000 : Points;
            }

            if (Points == -1)
            {
                if (index == indexRight)
                {
                    return 3000;
                }
                return 0;
            }

            if (index == indexRight)
            {
                return Points;
            }
            return -Points;
        }
    }
}
