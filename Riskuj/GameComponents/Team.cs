using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.GameComponents
{
    public class Team
    {
        public string Name { get; private set; }
        private int points;
        public Team(string name)
        {
            this.Name = name;
            this.points = 0;
        }   

        public void AddPoints (int points)
        {
            this.points += points;
        }

        public (string, int) Score()
        {
            return (Name, points);
        }
    }
}
