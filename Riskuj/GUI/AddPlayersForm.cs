using Riskuj.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riskuj.GUI
{
    public partial class AddPlayersForm : Form
    {
        private bool forceClose = false;
        private GameManager gameManager;
        public bool Success = false;
        public AddPlayersForm(GameManager? gameManager)
        {
            InitializeComponent();
            
            if (gameManager == null)
            {
                throw new Exception("Impossible to happen, GameManager is null");
            }

            this.gameManager = gameManager;
            this.NameTextBox.Focus();
        }

        private void AddPlayersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (this.NameTextBox.Text == "")
            {
                this.NameTextBox.Focus();
                return;
            }

            List<string> teamNames = gameManager.Score().Select(x => x.Item1).ToList();
            if (teamNames.Where(x => x == this.NameTextBox.Text).Any())
            {
                MessageBox.Show("Jméno již používáno");
                this.NameTextBox.Focus();
                return;
            }

            if (this.NameTextBox.Text.Length > 18)
            {
                MessageBox.Show("Příliš dlouhé jméno, maximální délka jména je 18 znaků");
                this.NameTextBox.Focus();
                return;
            }

            gameManager.AddTeam(this.NameTextBox.Text);
            teamNames = gameManager.Score().Select(x => x.Item1).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("Týmy:\n");

            foreach (string teamName in teamNames)
            {
                sb.Append("  " + teamName + "\n");
            }

            this.NameTextBox.Text = "";            
            this.TeamsLabel.Text = sb.ToString();
            this.NameTextBox.Focus();
        }
        private void EndButton_Click_1(object sender, EventArgs e)
        {
            var window = MessageBox.Show("Máte opravdu všechny tými přidány?", "Warning", MessageBoxButtons.YesNo);
            if (window == DialogResult.No)
            {
                return;
            }

            if (gameManager.Score().Count() < 2)
            {
                MessageBox.Show("Musíte mít minimálně 2 týmy");
                return;
            }

            forceClose = true;
            Success = true;
            this.Close();
        }
    }
}
