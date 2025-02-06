using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riskuj.Exception_;

namespace Riskuj.GameComponents
{
    public class Domain
    {
        public string Name { get; set; }
        public IEnumerable<Question> Questions { get; }
        public Domain (string name, IEnumerable<Question> questions)
        {
            if (questions.Count() < 5)
            {
                throw new NotValidGameFile();
            }
            Name = name;
            Questions = questions;
        }
        public Question GetQuestion (int points)
        {
            return Questions.Where(q => q.Points == points).First();
        }
        public (string, IEnumerable<bool>) GetFreeQuestions()
        {
            return (Name, Questions.Select(q => q.Answered).ToList());
        }
    }
}
