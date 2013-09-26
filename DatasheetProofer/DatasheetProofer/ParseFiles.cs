using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Windows.Forms;

namespace DatasheetProofer
{
    class ParseFiles
    {
        // select tags/markers to identify the table content to be parsed
        static private string [] keywordTags = {"Software Codes", "Note: Table 3", };

        static public string[,] LoadDataSheet(string datasheetFileName)
        {
            Excel._Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range startTag = null;
            Excel.Range endTag = null;

            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(datasheetFileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // set the find range as col A
            Excel.Range tableTitles = (Excel.Range)xlWorkSheet.Columns["A:B", Type.Missing];

            // search keywords in the datasheet
            startTag = tableTitles.Find(keywordTags[0],
                xlWorkSheet.Cells[1, 1],
                Excel.XlFindLookIn.xlValues,
                Excel.XlLookAt.xlPart,
                Excel.XlSearchOrder.xlByRows,
                Excel.XlSearchDirection.xlNext,
                false, false, false);
            endTag = tableTitles.Find(keywordTags[1],
                xlWorkSheet.Cells[1, 1],
                Excel.XlFindLookIn.xlValues,
                Excel.XlLookAt.xlPart,
                Excel.XlSearchOrder.xlByRows,
                Excel.XlSearchDirection.xlNext,
                false, false, false);

            //get the right position for the software codes table
            //            string sAddress = startTag.get_Address(false, false, Excel.XlReferenceStyle.xlA1, false, false);
            //            string eAddress = endTag.get_Address(false, false, Excel.XlReferenceStyle.xlA1, false, false);
            int[] sPos = { startTag.Row + 1, startTag.Column };
            int[] ePos = { endTag.Row - 1, endTag.Column };

            //            string result = string.Empty;
            int cols = 8;
            string[,] specsTable = new string[ePos[0] - sPos[0] + 1, cols];
            for (int i = 0; i <= ePos[0] - sPos[0]; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    object rangeObject = xlWorkSheet.Cells[sPos[0] + i, sPos[1] + j];
                    Excel.Range range = (Excel.Range)rangeObject;
                    object rangeValue = range.Value2;
                    if (rangeValue == null)
                    {
                        specsTable[i, j] = null;
                    }
                    else
                    {
                        specsTable[i, j] = rangeValue.ToString();
                    }
                    //result += " " + cellValue;
                }
            }
            // read rows for each products
            return specsTable;
        }

        static public void LoadScriptFiles(string scriptFolderPath)
        {

            foreach (string fileName in Directory.GetFiles(scriptFolderPath, "*.ini", SearchOption.AllDirectories))
            {
                ReadTestScriptFile(fileName);
            }

    
        }

        static public bool ReadTestScriptFile(string scriptFileName)
        {
            int scriptFileLinesCount;
            int numberOfTestPoints = 0;
            string[] testType___;
            string[] testName___;
            string[] testUnit___;
            string[] testTarget_;
            string[] testSpecMin;
            string[] testSpecMax;

            string[] fileToLinesDelimiters = new[] { "\r\n" };
            string[] lineToParamsDelimiters = new[] { "\t", "," };
            string scriptFileRead;
            List<string> scriptFileLines = new List<string>();
            string[] scriptFileLineParams;

            try
            {
                StreamReader sr = new StreamReader(scriptFileName);
                scriptFileRead = sr.ReadToEnd();
                sr.Close();

                if (!scriptFileRead.Equals(string.Empty))
                {
                    scriptFileLines.AddRange(scriptFileRead.Split(fileToLinesDelimiters, StringSplitOptions.RemoveEmptyEntries));
                    scriptFileLinesCount = scriptFileLines.Count;
                    testType___ = new string[scriptFileLinesCount];
                    testName___ = new string[scriptFileLinesCount];
                    testUnit___ = new string[scriptFileLinesCount];
                    testTarget_ = new string[scriptFileLinesCount];
                    testSpecMin = new string[scriptFileLinesCount];
                    testSpecMax = new string[scriptFileLinesCount];

                    for (int scriptTestOrderIndex = 1; scriptTestOrderIndex < scriptFileLinesCount; scriptTestOrderIndex++)
                    {
                        if (scriptFileLines[scriptTestOrderIndex].Substring(0, 1) != "'" && (!(scriptFileLines[scriptTestOrderIndex].Contains("*") && scriptFileLines.Contains("="))))
                        {
                            scriptFileLineParams = scriptFileLines[scriptTestOrderIndex].Split(lineToParamsDelimiters, StringSplitOptions.None);
                            if (scriptFileLineParams[3].Trim().ToUpper() == "BIN")
                                // skip HW&SW bins collecting
                                continue;
                            else if (scriptFileLineParams[0].Trim().Equals("1"))
                            {
                                for (int lineParamIndex = 0; lineParamIndex < 12; lineParamIndex++)
                                {
                                    if (lineParamIndex == 2 && scriptFileLineParams.Length > 2) testType___[numberOfTestPoints] = scriptFileLineParams[2].Trim().ToUpper();
                                    if (lineParamIndex == 3 && scriptFileLineParams.Length > 3) testName___[numberOfTestPoints] = scriptFileLineParams[3].Trim().ToUpper();
                                    if (lineParamIndex == 4 && scriptFileLineParams.Length > 4) testUnit___[numberOfTestPoints] = scriptFileLineParams[4].Trim().ToUpper();
                                    if (lineParamIndex == 7 && scriptFileLineParams.Length > 7) testTarget_[numberOfTestPoints] = scriptFileLineParams[7].Trim().ToUpper();
                                    if (lineParamIndex == 8 && scriptFileLineParams.Length > 8) testSpecMin[numberOfTestPoints] = scriptFileLineParams[8].Trim().ToUpper();
                                    if (lineParamIndex == 9 && scriptFileLineParams.Length > 9) testSpecMax[numberOfTestPoints] = scriptFileLineParams[9].Trim().ToUpper();
                                }
                            }
                            numberOfTestPoints++;
                        } // if line starts with "'", skip it
                    } // for loop of each line scanning
                }
                return true;
            }
            catch (Exception er)
            {
                MessageBox.Show("readScriptFile process failed: \n" + er.ToString());
                return false;
            }
        }

    }
}
