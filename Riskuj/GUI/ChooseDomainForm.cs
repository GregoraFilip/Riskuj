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
    public partial class ChooseDomainForm : Form
    {
        public bool Success = false;
        public string Chosen = "";
        private bool forceClose = false;
        public ChooseDomainForm(List<string> chooseCombo)
        {
            InitializeComponent();
            ChooseDeleteComboBox.Items.Clear();
            ChooseDeleteComboBox.Items.Add("");
            foreach (string item in chooseCombo)
            {
                ChooseDeleteComboBox.Items.Add(item);
            }
            ChooseDeleteComboBox.SelectedIndex = 0;
            ChooseDeleteComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Chosen = (string) ChooseDeleteComboBox.SelectedItem;
            if (Chosen != "")
            {
                Success = true;
            }
            forceClose = true;
            this.Close();
        }

        private void DeleteDomainForm_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
