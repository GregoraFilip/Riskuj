using Riskuj.Enum;
using Riskuj.Exception_;
using Riskuj.GameComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Riskuj.DBContext
{
    public class GameDBContext
    {
        private string path;
        public GameDBContext(string path) 
        {
            this.path = path;
        }
        public void CreateNewGame(GameModeEnum mode)
        {
            Game game = new Game(mode, new List<Domain>());
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using var fs = File.Create(path);
            fs.Close();
            SaveGame(game);
        }
        public GameModeEnum GetGameMode()
        {
            Game game = LoadGame();
            return game.GameMode;
        }
        public void SaveGame(Game game)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(game, options);
            File.Delete(path);
            using StreamWriter outputFile = new StreamWriter(path);
            outputFile.WriteLine(jsonString);
        }
        public Game LoadGame()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(path))
            {
                line = inputFile.ReadToEnd();
            }

            if (line == null)
            {
                throw new System.Text.Json.JsonException();
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var model = JsonSerializer.Deserialize<Game>(line, options);

            return model;
        }
    }
}
