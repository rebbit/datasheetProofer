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
        string[,] specsTable;
        VerificationStatus[,] specsTableStatus;

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
                specsTable = new string[,] { };
                string datasheetFileName = openFileDialog1.FileName.ToString();
                specsTable = FileParser.LoadDataSheet(datasheetFileName);
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
                int rowCount = specsTable.GetLength(0);
                int rowLength = specsTable.GetLength(1);
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
                specsTableStatus = new VerificationStatus[,]{};
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    specsTableStatus = FileParser.LoadScriptFiles(dialog.SelectedPath);
                    // update front/bg colors based on the status table
                    UpdateSpecsTable();
                }
            }


        }

        private void UpdateSpecsTable()
        {
            int rowCount = specsTableStatus.GetLength(0);
            int colCount = specsTableStatus.GetLength(1);
            for (int i = 1; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    switch (specsTableStatus[i, j])
                    {
                        case VerificationStatus.GREEN:
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                            break;
                        case VerificationStatus.RED:
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            break;
                        case VerificationStatus.GRAY:
                        default:
//                            dataGridView1.Rows[i].Cells[j].Style.FontColor = Color.Black;
                            break;
                    }

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
