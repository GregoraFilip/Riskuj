using Riskuj.Enum;
using Riskuj.Exception_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskuj.GameComponents
{
    public class Game
    {
        public GameModeEnum GameMode { get; }
        public IEnumerable<Domain> Domains { get; }
        public List<Team> Teams { get; } = new List<Team>();
        private int index = 0;
        private bool playCalled = false;
        public Game(GameModeEnum gameMode, IEnumerable<Domain> domains) 
        {
            GameMode = gameMode;
            Domains = domains;
        }

        public Team NextTeam()
        {
            if (!playCalled)
            {
                play();
            }

            var result = Teams.ElementAt(index++ % Teams.Count);
            index = index++ % Teams.Count;
            return result;
        }

        public void AddTeam(Team t)
        {
            Teams.Add(t);
        }

        private void play()
        {
            if (Teams.Count == 0)
            {
                throw new NoTeamAddedException();
            }

            playCalled = true;
            index = new Random().Next(0, Teams.Count);
        }

        public List<(string, int)> Score()
        {
            return Teams.Select(t => t.Score()).ToList();
        }
    }
}
