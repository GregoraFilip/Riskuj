using Riskuj.GameComponents;
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
    public partial class AddBonusDomainForm : Form
    {
        public AddBonusDomainForm()
        {
            InitializeComponent();
        }
        public AddBonusDomainForm(Domain domain)
        {
            InitializeComponent();
            Questions = domain.Questions.ToList();
            NameTextBox1.Text = domain.Name;
            this._1000Button.BackColor = Color.DarkGreen;
            this._2000Button.BackColor = Color.DarkGreen;
            this._3000Button.BackColor = Color.DarkGreen;
            this._4000Button.BackColor = Color.DarkGreen;
            this._5000Button.BackColor = Color.DarkGreen;
            this.bonusButton.BackColor = Color.DarkGreen;
        }

        protected bool forceClose = false;
        public List<Question> Questions = new List<Question>();
        public string NameD = "";
        public bool Success = false;
        public Question? BonusQuestion = null;
        virtual protected void button_clicked(Button button)
        {
            TextQuestionForm questionForm;
            int points = -1;
            if (button.Text != "BONUS")
            {
                points = Int32.Parse(button.Text);
            }

            var q = Questions.Where(q => q.Points == points);
            if (q.Count() == 0)
            {
                questionForm = new TextQuestionForm(points == -1 ? "BONUS" : points.ToString());
            }
            else if (q.First().Answers.Count() == 0)
            {
                questionForm = new TextQuestionForm(q.First().Text, points == -1 ? "BONUS" : points.ToString());
            }
            else
            {
                questionForm = new TextQuestionForm(q.First().Text, q.First().Answers.ToList(), points == -1 ? "BONUS" : points.ToString());
            }

            questionForm.ShowDialog();
            if (questionForm.Success && questionForm.Options.Count() == 4)
            {
                Questions.RemoveAll(q => q.Points == points);
                Questions.Add(new Question(points, questionForm.Question, questionForm.Options));
                button.BackColor = Color.DarkGreen;
            }
            else if (questionForm.Success && questionForm.Options.Count() == 0) 
            {
                Questions.RemoveAll(q => q.Points == points);
                Questions.Add(new Question(points, questionForm.Question, new List<Answer>()));
                button.BackColor = Color.DarkGreen;
            }
        }

        protected void _1000Button_Click(object sender, EventArgs e)
        {
            button_clicked(this._1000Button);
        }

        protected void _2000Button_Click(object sender, EventArgs e)
        {
            button_clicked(this._2000Button);
        }

        protected void _3000Button_Click(object sender, EventArgs e)
        {
            button_clicked(this._3000Button);
        }

        protected void _4000Button_Click(object sender, EventArgs e)
        {
            button_clicked(this._4000Button);
        }

        protected void _5000Button_Click(object sender, EventArgs e)
        {
            button_clicked(this._5000Button);
        }

        private void bonusButton_Click(object sender, EventArgs e)
        {
            button_clicked((Button)sender);
        }

        protected void AddNormalDomain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            forceClose = true;
            this.Close();
        }

        virtual protected void OkButton_Click(object sender, EventArgs e)
        {
            if (NameTextBox1.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplnit jméno oblasti.");
                return;
            }
            if (Questions.Count() != 6)
            {
                MessageBox.Show("Zapomněli jste vyplnit některou otázku.");
                return;
            }

            NameD = NameTextBox1.Text;
            Success = true;

            forceClose = true;
            this.Close();
        }
    }
}
