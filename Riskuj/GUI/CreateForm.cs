using Riskuj.DBContext;
using Riskuj.Enum;
using Riskuj.Exception_;
using Riskuj.GameComponents; 
using Riskuj.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riskuj.GUI
{
    public partial class CreateForm : Form
    {
        private bool forceClose = false;
        private string path;
        private GameModeEnum gameMode;
        private GameCreator gameCreator;
        public CreateForm(string path, GameModeEnum gameMode, out bool repeat)
        {
            InitializeComponent();
            repeat = true;
            this.gameMode = gameMode;
            this.path = path;

            gameCreator = new GameCreator(path);
            if (this.gameMode == GameModeEnum.Edit)
            {
                this.gameMode = gameCreator.GetGameMode();
            }
            else
            {
                gameCreator.CreateNewGame(this.gameMode);
            }

            repeat = false;
        }


        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            var gameManager = new GameManager(gameCreator.GetPreview(), true);
            var gameForm = new GameForm(gameManager);
            gameForm.Show();
        }

        private async void EditQuestionButton_Click(object sender, EventArgs e)
        {
            List<string> domainNames = gameCreator.GetDomaisName();
            var chooseDomainForm = new ChooseDomainForm(domainNames);

            chooseDomainForm.ShowDialog();

            string formerName = "";
            if (chooseDomainForm.Success)
            {
                Riskuj.GameComponents.Domain? domain = null;
                try
                {
                    domain = gameCreator.GetDomain(chooseDomainForm.Chosen);
                }
                catch (System.Text.Json.JsonException)
                {
                    MessageBox.Show("File you are trying read from is not a valid Riskuj-Game File.\n" +
                        "Revert changes you did in this file and try again.");
                    return;
                }
                if (domain == null)
                {
                    MessageBox.Show("Domain you are requesting isn't in file anymore.");
                    return;
                }

                formerName = domain.Name;

                if (gameMode == GameModeEnum.Normal)
                {
                    var normalDomainForm = new AddNormalDomainForm(domain);
                    normalDomainForm.ShowDialog();
                    if (normalDomainForm.Success)
                    {
                        domain = new Riskuj.GameComponents.Domain(normalDomainForm.NameD, normalDomainForm.Questions);
                    }
                }
                else if (gameMode == GameModeEnum.Bonus)
                {
                    var bonusDomainForm = new AddBonusDomainForm(domain);
                    bonusDomainForm.ShowDialog();
                    if (bonusDomainForm.Success)
                    {
                        domain = new Riskuj.GameComponents.Domain(bonusDomainForm.NameD, bonusDomainForm.Questions);
                    }
                }

                try
                {
                    gameCreator.ReplaceDomain(formerName, domain);
                }
                catch (System.Text.Json.JsonException)
                {
                    MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                        "Revert changes you did in this file and try again.");
                }
                catch (NotValidGameFile)
                {
                    MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                          "Revert changes you did in this file and try again.");
                }
                catch (IOException)
                {
                    MessageBox.Show("You don't have permission to access this file.\n" +
                        "Revert changes you did in this file and try again.");
                }
                catch (TooManySameNameException)
                {
                    MessageBox.Show("Imposible to add nine domains of the same name.");
                }
            }
        }

        private void AddDomainButton_Click(object sender, EventArgs e)
        {
            Riskuj.GameComponents.Domain? domain = null;
            if (gameMode == GameModeEnum.Normal)
            {
                var normalDomainForm = new AddNormalDomainForm();
                normalDomainForm.ShowDialog();
                if (normalDomainForm.Success)
                {
                    domain = new Riskuj.GameComponents.Domain(normalDomainForm.NameD, normalDomainForm.Questions);
                }
            }
            else if (gameMode == GameModeEnum.Bonus)
            {
                var bonusDomainForm = new AddBonusDomainForm();
                bonusDomainForm.ShowDialog();
                if (bonusDomainForm.Success)
                {
                    domain = new Riskuj.GameComponents.Domain(bonusDomainForm.NameD, bonusDomainForm.Questions);
                }
            }
            if (domain == null)
            {
                return;
            }
            try
            {
                gameCreator.AddDomain(domain);
            }
            catch (System.Text.Json.JsonException)
            {
                MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                    "Revert changes you did in this file and try again.");
            }
            catch (NotValidGameFile)
            {
               MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                     "Revert changes you did in this file and try again.");
            }
            catch (IOException)
            {
                MessageBox.Show("You don't have permission to access this file.\n" +
                    "Revert changes you did in this file and try again.");
            }
            catch (TooManySameNameException)
            {
                MessageBox.Show("Imposible to add Nineth domain of the same name.");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            List<string> domainNames = gameCreator.GetDomaisName();
            var chooseDomainForm = new ChooseDomainForm(domainNames);
            chooseDomainForm.ShowDialog();

            if (chooseDomainForm.Success)
            {
                try
                {
                    gameCreator.DeleteDomain(chooseDomainForm.Chosen);
                }
                catch (System.Text.Json.JsonException)
                {
                    MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                        "Revert changes you did in this file and try again.");
                }
                catch (NotValidGameFile)
                {
                    MessageBox.Show("File you are trying write into is not a valid Riskuj-Game File.\n" +
                          "Revert changes you did in this file and try again.");
                }
                catch (IOException)
                {
                    MessageBox.Show("You don't have permission to access this file.\n" +
                        "Revert changes you did in this file and try again.");
                }
            }

        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            forceClose = true;
            this.Close();
        }


    }
}
