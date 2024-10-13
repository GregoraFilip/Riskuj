using System.Text.Json;
using Riskuj.GameComponents;
using Riskuj.Exception_;
using Riskuj.Enum;
using Riskuj.DBContext;
using System.Windows.Forms;

namespace Riskuj.Utils
{
    class GameCreator
    {
        private object creatorLock = (object) 1;
        private GameDBContext gameDBContext;
        private GameModeEnum gameMode = GameModeEnum.Exit;
        private Dictionary<int, string> intToRoma = new Dictionary<int, string>();

        public GameCreator(string path) 
        {
            gameDBContext = new GameDBContext(path);

            intToRoma[1] = "I";
            intToRoma[2] = "II";
            intToRoma[3] = "III";
            intToRoma[4] = "IV";
            intToRoma[5] = "V";
            intToRoma[6] = "VI";
            intToRoma[7] = "VII";
            intToRoma[8] = "VIII";
        }
        public void CreateNewGame(GameModeEnum gameMode)
        {
            gameDBContext.CreateNewGame(gameMode);
            this.gameMode = gameMode;
        }

        public GameModeEnum GetGameMode ()
        {
            gameMode = gameDBContext.GetGameMode();
            return gameMode;
        }

        public List<string> GetDomaisName()
        {
            Game game = gameDBContext.LoadGame();
            return game.Domains.Select(d => d.Name).ToList();
        }
        public Domain? GetDomain(string name)
        {
            var q = gameDBContext.LoadGame().Domains.Where(d => d.Name == name);
            if (!q.Any())
            {
                return null;
            }
            return q.First();
        }

        public void DeleteDomain(string name)
        {
            lock (creatorLock)
            {
                Game game = gameDBContext.LoadGame();
                game = new Game(game.GameMode, game.Domains.Where(d => d.Name != name));
                gameDBContext.SaveGame(game);
            }
        }

        public void ReplaceDomain(string name, Domain domain)
        {
            lock (creatorLock)
            {
                var game = gameDBContext.LoadGame();
                var domains = game.Domains.Where(d => d.Name != name).ToList();
                domains.Add(domain);
                gameDBContext.SaveGame(new Game(game.GameMode, domains));
            }
        }
        public void AddDomain(Domain domain)
        {
            lock (creatorLock)
            {
                Game game = gameDBContext.LoadGame();
                var domains = game.Domains.ToList();
                string newName = domain.Name;
                int i = 1;
                while (domains.Where(d => d.Name == newName).Count() != 0)
                {
                    if (i > 8) { throw new TooManySameNameException(); }

                    newName = domain.Name + " " + intToRoma[i] + ".";
                    i++;
                }

                domain.Name = newName;
                domains.Add(domain);
                gameDBContext.SaveGame(new Game(game.GameMode, domains));
            }
        }

        public Game GetPreview()
        {
            return gameDBContext.LoadGame();
        }
    }
}
