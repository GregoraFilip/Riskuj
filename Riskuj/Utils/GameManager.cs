using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riskuj.GameComponents;
using Riskuj.DBContext;
using Riskuj.Enum;

namespace Riskuj.Utils
{
    public class GameManager
    {
        private Game game;
        private Team? playingTeam;
        private List<Team> possibleTeams = new List<Team>();
        private bool preview = false;
        public GameManager(Game g)
        {
            game = g;
        }

        public GameManager(Game g, bool preview)
        {
            game = g;
            this.preview = preview;
        }

        public GameManager(string path)
        {
            game = new GameDBContext(path).LoadGame();
        }

        public void AddTeam(string name)
        {
            game.AddTeam(new Team(name));
        }

        public List<(string, int)> Score()
        {
            if (preview) return new List<(string, int)> { ("", 0) };
            return game.Score();
        }

        public GameModeEnum GetGameMode()
        {
            return game.GameMode;
        }

        public (bool, List<string>) QuestionAnswered(int i, Question question)
        {
            int points = question.Answer(i);
            if (preview) return (question.Answers.Where(a => !a.Chosen).Any(), new List<string> { "Vidět další" });
            playingTeam.AddPoints(points);
            return (points < 0  && possibleTeams.Count() != 0, possibleTeams.Select(t => t.Name).ToList());
        }

        public bool SelectedTeam(string name)
        {
            if (preview) return name != "Vidět další" ? false : true;
            if (possibleTeams.Where(t => t.Name == name).Count() == 0)
            {
                return false;
            }
            playingTeam = possibleTeams.Where(t => t.Name == name).First();
            possibleTeams = possibleTeams.Where(t => t.Name != name).ToList();
            return true;
        }

        public string NextTeam()
        {
            if (preview) return "";
            playingTeam = game.NextTeam();
            possibleTeams = game.Teams.Where(t => t.Name != playingTeam.Name).ToList();
            return playingTeam.Name;
        }

        public IEnumerable<Domain> Domains()
        {
            return game.Domains;
        }

        public bool  AllQuestionsAnswered()
        {
            return game.Domains.All(d => d.Questions.All(q => q.Answered));
        }

        public IEnumerable<string> GetBonusDomains()
        {
            return game.Domains.Where(d => d.Questions.Count(q => q.Answered) == 5).Select(d => d.Name);
        }

    }
}
