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

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDatasheet();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadScripts();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////
        private void LoadDatasheet()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Excel Files(*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string datasheetFileName = openFileDialog1.FileName.ToString();
                string[,] specsTable = FileParser.LoadDataSheet(datasheetFileName);
                //StringBuilder strBuilder = new StringBuilder();
                //for (int i = 0; i < specsTable.GetLength(0); i++)
                //{
                //    for (int j = 0; j < specsTable.GetLength(1) - 2; j++)
                //    {
                //        strBuilder.Append(specsTable[i, j] + "\t");
                //    }
                //    strBuilder.AppendLine();
                //}
                //textBox1.ReadOnly = false;
                //textBox1.Text = strBuilder.ToString();
                //textBox1.ReadOnly = true;

                //MessageBox.Show("Next: load your scripts to verify datasheet");
                //                openScriptToolStripMenuItem.Enabled = true;
                var rowCount = specsTable.GetLength(0);
                var rowLength = specsTable.GetLength(1);
                dataGridView1.ColumnCount = rowLength;
                for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
                {
                    var row = new DataGridViewRow();
                    string myVal;
                    for (int columnIndex = 0; columnIndex < rowLength; ++columnIndex)
                    {
                        myVal = specsTable[rowIndex, columnIndex];
                        if (myVal == null) myVal = string.Empty;
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = myVal });
                    }
                    dataGridView1.Rows.Add(row);
                }
                button2.Enabled = true;
            }
        }


        private void LoadScripts()
        {
            string startupPath = Application.StartupPath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Open a script folder";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    VerificationStatus[,] specsTableStatus = FileParser.LoadScriptFiles(dialog.SelectedPath);
                    // update front/bg colors based on the status table

                }
            }


        }

    }
}
