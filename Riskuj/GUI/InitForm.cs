using System.Text.Json.Nodes;
using System.Windows.Forms;
using Riskuj.Enum;

namespace Riskuj
{

    public partial class InitForm : Form
    {
        public string? Path { get; private set; }
        public InitActionEnum Action { get; private set; }
        private bool forceClose = false;
        public InitForm()
        {
            Action = InitActionEnum.Nothing;
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog() { Filter = "JSON file (*.json) | *.json" };
            var result = fd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            Action = InitActionEnum.Load;
            Path = fd.FileName;

            forceClose = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog() { Filter = "JSON file (*.json) | *.json" };
            var result = sfd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            Action = InitActionEnum.New;
            Path = sfd.FileName;

            forceClose = true;
            this.Close();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                var window = MessageBox.Show("Opravdu chcete odejít?", "Warning", MessageBoxButtons.YesNo);

                e.Cancel = window == DialogResult.No;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog() { Filter = "JSON file (*.json) | *.json" };
            var result = fd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            Action = InitActionEnum.Edit;
            Path = fd.FileName;

            forceClose = true;
            this.Close();
        }
    }
}