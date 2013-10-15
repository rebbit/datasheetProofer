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
        FileParser fileparser;
        ScriptParser scriptparser;

        public Form1()
        {
            InitializeComponent();
            fileparser = new FileParser();
            scriptparser = new ScriptParser();
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
            LoadScripts(specsTable);
        }

        private void loadDatasheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDatasheet();
        }

        private void loadScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadScripts(specsTable);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        ////////////////////////////////datssheet handling/////////////////////////////////////////////
        private void LoadDatasheet()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Excel Files(*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                specsTable = new string[,] { };
                string datasheetFileName = openFileDialog1.FileName.ToString();
                fileparser.LoadDataSheet(datasheetFileName, out specsTable);
                ShowDatasheet();

                //enable load scripts button and menu item for next step.
                loadScriptsButton.Enabled = true;
                loadScriptsToolStripMenuItem.Enabled = true;
            }
        }

        private void ShowDatasheet()
        {
            int rows = specsTable.GetLength(0);
            int cols = specsTable.GetLength(1);
            dataGridView1.ColumnCount = cols;
            for (int i = 0; i < rows; ++i)
            {
                var row = new DataGridViewRow();
                string myVal;
                for (int j = 0; j < cols; ++j)
                {
                    myVal = specsTable[i, j];
                    if (myVal == null) myVal = string.Empty;
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = myVal });
                }
                dataGridView1.Rows.Add(row);
            }
        }

        ////////////////////////////////scripts handling/////////////////////////////////////////////
        private void LoadScripts(string[,] specsTable)
        {
            string startupPath = Application.StartupPath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Open a script folder";
                dialog.ShowNewFolderButton = true;
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                specsTableStatus = new VerificationStatus[,]{};
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    scriptparser.LoadScriptFiles(dialog.SelectedPath, specsTable, out specsTableStatus);
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
