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
    public partial class SelectTeamForm : Form
    {
        public string Chosen = "";
        public SelectTeamForm(List<string> teamNames)
        {
            InitializeComponent();
            comboBox1.Items.Add("None");
            foreach (string name in teamNames)
            {
                comboBox1.Items.Add(name);
            }
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ChosenButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Chosen = "";
            }
            Chosen = (string)comboBox1.SelectedItem;
            this.Close();
        }
    }
}
