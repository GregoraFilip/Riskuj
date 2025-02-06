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
using System.Xml.Linq;

namespace Riskuj.GUI
{
    public partial class TextQuestionForm : Form
    {
        private bool forceClose = false;
        public List<Answer> Options = new List<Answer>();
        public string Question = "";
        public bool Success = false;
        public TextQuestionForm()
        {
            InitializeComponent();
        }

        public TextQuestionForm(string name)
        {
            InitializeComponent();
            this.Text = name;
        }

        public TextQuestionForm(string text, string name)
        {
            InitializeComponent();
            this.Text = name;
            TextTextBox.Text = text;
        }

        public TextQuestionForm(string text, List<Answer> options)
        {
            InitializeComponent();
            TextTextBox.Text = text;
            RightAnswerText.Text = options.ElementAt(0).Text;
            Option2Text.Text = options.ElementAt(1).Text;
            Option3Text.Text = options.ElementAt(2).Text;
            Option4Text.Text = options.ElementAt(3).Text;
        }

        public TextQuestionForm(string text, List<Answer> options, string name)
        {
            InitializeComponent();
            TextTextBox.Text = text;
            RightAnswerText.Text = options.ElementAt(0).Text;
            Option2Text.Text = options.ElementAt(1).Text;
            Option3Text.Text = options.ElementAt(2).Text;
            Option4Text.Text = options.ElementAt(3).Text;
            this.Text = name;
        }


        private void TextQuestionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            forceClose = true;
            this.Close();
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            if (TextTextBox.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit otázku.");
                return;
            }
            else if (RightAnswerText.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit správnou odpověď.");
                return;
            }
            else if (Option2Text.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit možnost 2.");
                return;
            }
            else if (Option3Text.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit možnost 3.");
                return;
            }
            else if (Option4Text.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit možnost 4.");
                return;
            }

            Options.Add(new Answer(RightAnswerText.Text));
            Options.Add(new Answer(Option2Text.Text));
            Options.Add(new Answer(Option3Text.Text));
            Options.Add(new Answer(Option4Text.Text));
            Question = TextTextBox.Text;
            Success = true;

            forceClose = true;
            this.Close();
    }

        private void brickButton_Click(object sender, EventArgs e)
        {
            if (TextTextBox.Text == "")
            {
                MessageBox.Show("Zapomněli jste vyplit jméno cihličky.");
                return;
            }

            Question = TextTextBox.Text;
            Success = true;

            forceClose = true;
            this.Close();
        }
    }
}
