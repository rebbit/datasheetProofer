using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatasheetProofer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openDatasheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void openDatasheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Excel Files(*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string datasheetFileName = openFileDialog1.FileName.ToString();
                MessageBox.Show(ParseExcel.LoadDataSheet(datasheetFileName));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
