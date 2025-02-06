using Riskuj.Enum;
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
    public partial class GameModeForm : Form
    {
        public GameModeEnum Mode { get; private set; }
        private bool forceClose = false;
        public GameModeForm()
        {
            Mode = GameModeEnum.Exit;
            InitializeComponent();
        }

        private void BonusButton_Click(object sender, EventArgs e)
        {
            Mode = GameModeEnum.Bonus;
            forceClose = true;
            this.Close();
        }

        private void NormalButton_Click(object sender, EventArgs e)
        {
            Mode = GameModeEnum.Normal;
            forceClose = true;
            this.Close();
        }

        private void GameModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít? Všechny údaje budou ztraceny", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }
    }
}
