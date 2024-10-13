using Riskuj.GameComponents;
using Riskuj.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Riskuj.GUI
{
    public partial class ResultForm : Form
    {
        private int i = 2;
        private GameManager gameManager;
        public ResultForm(GameManager gameManager)
        {
            InitializeComponent();
            this.gameManager = gameManager;
            this.FirstLabel.Text = "";
            this.SecondLabel.Text = "";
            this.ThirdLabel.Text = "";

            printBadPlaces();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (gameManager.Score().Count > i && i >= 0)
            {
                (string name, int points) = gameManager.Score().OrderByDescending(t => t.Item2).ElementAt(i);
                sb.Append(new String(' ', (18 - name.Length + 1) / 2));
                sb.Append(name);
                sb.Append(new String(' ', (18 - name.Length) / 2));
                sb.Append('\n');
                sb.Append(new String(' ', 6));
                if (Math.Abs(points) < 10000 || points > 0) { sb.Append(' '); }
                if (points == 0) { sb.Append(new String(' ', 2)); }
                sb.Append(points.ToString());
            }
            else { i--;  return; }

            if (i == 2)
            {
                this.ThirdLabel.Text = sb.ToString();
            }
            else if (i == 1)
            {
                this.SecondLabel.Text = sb.ToString();
            } else if (i == 0)
            {
                this.FirstLabel.Text = sb.ToString();
                timer1.Stop();
            }

            i--;
        }

        private void printBadPlaces ()
        {
            StringBuilder sb = new StringBuilder();

            var namePoints = gameManager.Score().OrderByDescending(t => t.Item2).Reverse().Take(gameManager.Score().Count - 3).Reverse();
            int longestName = 0;
            if (namePoints.Count() > 0)
            {
                longestName = namePoints.Select(i => i.Item1.Length).Max();
            }
            i = 4;
            foreach ((string name, int points) in namePoints)
            {
                sb.Append(i.ToString() + ". ");
                i++;
                sb.Append(name);
                sb.Append(new String(' ', longestName - name.Length + 1));
                if (points > 0) { sb.Append(' '); }
                if (points == 0) { sb.Append(new String(' ', 3));  }
                if (Math.Abs(points) < 10000) { sb.Append(' '); }
                sb.Append(points.ToString());
                sb.Append("\n");
            }
            this.OtherTeamsLabel.Text = sb.ToString();
        }
    }
}
