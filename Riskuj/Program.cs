using Riskuj.Enum;
using Riskuj.Exception_;
using Riskuj.GUI;
using Riskuj.Utils;

namespace Riskuj
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //var gameManager2 = new GameManager("C:/Users/Fíla/Desktop/lkdjsa.json");
            //gameManager2.AddTeam("Test1");
            //gameManager2.AddTeam("Test2");
            //Application.Run(new NormalGameForm(gameManager2));

            bool repeat = true;
            while (repeat)
            {
                repeat = false;
                var initForm = new InitForm();
                Application.Run(initForm);

                if (initForm.Action == InitActionEnum.Load && initForm.Path != null)
                {
                    GameManager? gameManager = null;
                    try
                    {
                        gameManager = new GameManager(initForm.Path);
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        repeat = true;
                        MessageBox.Show("Not a valid Riskuj-Game File");
                    }
                    catch (NotValidGameFile)
                    {
                        repeat = true;
                        MessageBox.Show("Not a valid Riskuj-Game File");
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("You don't have permission to read there or file doesn't exist");
                    }

                    if (!repeat)
                    {
                        var addPlayerForm = new AddPlayersForm(gameManager);
                        Application.Run(addPlayerForm);
                        if (gameManager != null && addPlayerForm.Success && !gameManager.AllQuestionsAnswered())
                        {
                            Application.Run(new GameForm(gameManager));
                            Application.Run(new ResultForm(gameManager));
                        }
                    }
                }
                else if (initForm.Action == InitActionEnum.New && initForm.Path != null)
                {
                    var gameMode = new GameModeForm();
                    Application.Run(gameMode);
                    try
                    {
                        if (gameMode.Mode != GameModeEnum.Exit)
                        {
                            Application.Run(new CreateForm(initForm.Path, gameMode.Mode, out repeat));
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("You don't have permission to write there");
                    }
                }
                else if (initForm.Action == InitActionEnum.Edit && initForm.Path != null)
                {
                    try
                    {
                        Application.Run(new CreateForm(initForm.Path, GameModeEnum.Edit, out repeat));
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        MessageBox.Show("Not a valid Riskuj-Game File");
                    }
                    catch (NotValidGameFile)
                    {
                        MessageBox.Show("Not a valid Riskuj-Game File");
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("You don't have permission to read there or file doesn't exist");
                    }
                }
            }
        }
    }
}