using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riskuj.Utils;
using Riskuj.GameComponents;
using Riskuj.Enum;
using System.Reflection;

namespace Riskuj.GUI
{
    public partial class GameForm : Form
    {
        private SoundPlayer simpleSound;
        private GameManager gameManager;
        private bool forceClose = false;
        private List<(Button, string)> bonusButtons = new List<(Button, string)>();

        public GameForm(GameManager gameManager)
        {
            InitializeComponent();
            this.gameManager = gameManager;

            SuspendLayout();
            updateScore();
            updateNextTeam();
            createDomains();
            ResumeLayout();

            try 
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\music.wav";
                simpleSound = new SoundPlayer(path);
                simpleSound.Play();
            }
            catch (Exception)
            {
                ;
            }

        }

        private TableLayoutPanel getTbLayoutDomain(Domain d)
        {
            var tbLayout = new TableLayoutPanel
            {
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right };

            int i = 0;
            tbLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            tbLayout.Controls.Add(new Label { Text = d.Name,
                                              Font = new Font(new FontFamily("Courier New"), 13f),
                                              TextAlign = ContentAlignment.BottomCenter,
                                              Anchor = AnchorStyles.Top |
                                                        AnchorStyles.Bottom |
                                                        AnchorStyles.Left |
                                                        AnchorStyles.Right,
            }, 0, i);

            i++;
            foreach (Question question in d.Questions.OrderBy(q => q.Points).Where(q => q.Points != -1))
            {
                tbLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
                Button button = new Button {
                    Text = question.Points.ToString(),
                    Font = new Font(new FontFamily("Courier New"), 13f),
                    Anchor = AnchorStyles.Top
                             | AnchorStyles.Bottom
                             | AnchorStyles.Left
                             | AnchorStyles.Right,
                    Tag = (object)question
                };
                button.Click += buttonClicked;
                tbLayout.Controls.Add(button, 0, i);
                i++;
            }

            if (gameManager.GetGameMode() == GameModeEnum.Bonus)
            {
                var q = d.Questions.Where(q => q.Points == -1).First();
                tbLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
                Button button = new Button
                {
                    Text = "BONUS",
                    Font = new Font(new FontFamily("Courier New"), 13f),
                    Anchor = AnchorStyles.Top
                             | AnchorStyles.Bottom
                             | AnchorStyles.Left
                             | AnchorStyles.Right,
                    Tag = (object)q,
                    Enabled = false
                };
                button.Click += buttonClicked;
                tbLayout.Controls.Add(button, 0, i);
                bonusButtons.Add((button, d.Name));
            }

            return tbLayout;
        }

        private void createDomains()
        {
            this.MinimumSize = new Size(gameManager.Domains().Count() * 180 + 330, this.MinimumSize.Height);
            var tbLayout = new TableLayoutPanel
            {
                GrowStyle = TableLayoutPanelGrowStyle.AddColumns,
                Anchor = AnchorStyles.Top
                         | AnchorStyles.Bottom
                         | AnchorStyles.Left
                         | AnchorStyles.Right
            };

            int i = 0;
            foreach (Domain d in gameManager.Domains())
            {
                tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
                tbLayout.Controls.Add(getTbLayoutDomain(d), i, 0);
                i++;
            }

            tableLayoutPanel1.Controls.Add(tbLayout);
        }

        private void buttonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Question question = (Question)button.Tag;
            if (question == null)
            {
                return;
            }

            if (question.Answers.Count() == 0)
            {
                MessageBox.Show(question.Text);
                gameManager.QuestionAnswered(-1, question);
                updateScore();
            }
            else
            {
                bool firstTime = true;
                bool cont = true;
                List<string> teams = new List<string>();
                do
                {
                    if (!firstTime)
                    {
                        var selectTeamForm = new SelectTeamForm(teams);
                        selectTeamForm.ShowDialog();
                        cont = gameManager.SelectedTeam(selectTeamForm.Chosen);
                    }
                    else
                    {
                        firstTime = false;
                    }
                    if (cont)
                    {
                        TextQuestionFormShow textQuestionForm = new TextQuestionFormShow(question);
                        textQuestionForm.ShowDialog();
                        (cont, teams) = gameManager.QuestionAnswered(textQuestionForm.Chosen, question);
                        updateScore();
                    }
                } while (cont);
            }
            updateNextTeam();
            button.Enabled = false;
            button.BackColor = Color.DarkRed;
        }

        private async Task updateNextTeam()
        {
            this.NextTeamLabel.Text = "Další tým:\n  " + gameManager.NextTeam();
            if (gameManager.GetGameMode() == GameModeEnum.Bonus)
            {
                foreach (string n in gameManager.GetBonusDomains())
                {
                    bonusButtons.Where(i => i.Item2 == n).First().Item1.Enabled = true;
                }
            }

            if (gameManager.AllQuestionsAnswered())
            {
                await end();
            }
        }

        private async Task end()
        {
            forceClose = true;
            await Task.Delay(3000);
            this.Close();
        }

        private void updateScore()
        {
            StringBuilder sb = new StringBuilder();
            int longestName = gameManager.Score().Select(i => i.Item1.Length).Max();
            sb.Append("Body:\n");

            foreach ((string name, int points) in gameManager.Score())
            {
                string textPoints = "";
                if (points == 0)
                {
                    textPoints = new string(' ', 5) + points.ToString(); 
                }
                else if (points > 0 && points < 10000)
                {
                    textPoints = "  " + points.ToString();
                }
                else if (points >= 10000)
                {
                    textPoints = " " + points.ToString();
                }
                else if (points > -10000)
                {
                    textPoints = " " + points.ToString();
                }
                else
                {
                    textPoints = points.ToString();
                }

                sb.Append("  " + name + new String(' ', longestName - name.Length)  + ": " + textPoints + "\n");
            }
            this.PointsLabel.Text = sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void NormalGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (simpleSound != null)
            {
                simpleSound.Stop();
            }

            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
                if (window == DialogResult.No)
                {

                }
            }
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {

            if (simpleSound != null)
            {
                simpleSound.Stop();
            }
        }
    }
}