using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riskuj.GameComponents;

namespace Riskuj.GUI
{
    public partial class TextQuestionFormShow : Form
    {
        private bool forceClose = false;
        private Question question;
        public int Chosen = -1;
        private List<(Label, Button)> listLabelButton;
        public TextQuestionFormShow(Question question)
        {
            InitializeComponent();

            listLabelButton = new List<(Label, Button)>
            {
                 (this.aOptionLabel, this.aOptionButton),
                 (this.bOptionLabel, this.bOptionButton),
                 (this.cOptionLabel, this.cOptionButton),
                 (this.dOptionLabel, this.dOptionButton),
            };

            this.question = question;
            this.textQuestionlabel.Text = question.Text;

            
            for (int i = 0; i < question.GetAnswers().Count(); i++)
            {
                (Label label, Button button) = listLabelButton.ElementAt(i);
                label.Text = question.GetAnswers().ElementAt(i).Text;
                button.Tag = (object)i;
                if (question.GetAnswers().ElementAt(i).Chosen)
                {
                    button.Enabled = false;
                    button.BackColor = Color.DarkRed;
                }
            }
        }

        private async void chosenButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Chosen = (int)button.Tag;

            if (question.Answer(Chosen) > 0)
            {
                button.BackColor = Color.DarkGreen;
            }
            else
            {
                button.BackColor = Color.DarkRed;
            }
            
            foreach (Button b in listLabelButton.Select(t => t.Item2))
            {
                b.Enabled = false;
            }

            forceClose = true;
            await Task.Delay(2000);
            this.Close();
        }

        private void TextQuestionFormShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít? Všechny body budou ztraceny.", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }
    }
}
