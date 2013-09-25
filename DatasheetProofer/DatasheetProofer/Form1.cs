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
                string [,] specsTable = ParseFiles.LoadDataSheet(datasheetFileName);
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < specsTable.GetLength(0); i++)
                {
                    for (int j = 0; j < specsTable.GetLength(1) - 2; j++)
                    {
                        strBuilder.Append(specsTable[i, j] + "\t");
                    }
                    strBuilder.AppendLine();
                }
                textBox1.ReadOnly = false;
                textBox1.Text = strBuilder.ToString();
                textBox1.ReadOnly = true;

                MessageBox.Show("Next: load your scripts to verify datasheet");
                openScriptToolStripMenuItem.Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void openScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string startupPath = Application.StartupPath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Open a script folder";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.ReadOnly = false;
                    ParseFiles.LoadScriptFiles(dialog.SelectedPath);
                    textBox1.ReadOnly = true;

                }
            }

            //OpenFileDialog openFileDialog2 = new OpenFileDialog();
            //openFileDialog2.Filter = "All Script Files(*.ini)|*.ini";
            //if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    string scriptFileName = openFileDialog2.FileName.ToString();
            //    string[,] specsTable = ParseFiles.LoadScriptFiles(scriptFileName);
            //    StringBuilder strBuilder = new StringBuilder();
            //}

        }
    }
}
