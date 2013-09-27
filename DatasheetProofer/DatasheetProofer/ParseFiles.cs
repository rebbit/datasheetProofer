using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DatasheetProofer
{
    public enum VerificationStatus
    {
        GRAY,   //UNVERIFIED
        GREEN,  //PASSED,
        RED,    //FAILED
    }

    class ParseFiles
    {
        static private string productModel;
        static private string productVersion;

        static private string[,] softwareCodeTable;
        static private List<string[]> testParams = new List<string[]>();
        static private int numberOfTestPoints = 0;
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
            // add the additional last column for verification status (grey|red|green)
            int cols = 8 + 1;
            softwareCodeTable = new string[ePos[0] - sPos[0] + 1, cols];
            for (int i = 0; i <= ePos[0] - sPos[0]; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    object rangeObject = xlWorkSheet.Cells[sPos[0] + i, sPos[1] + j];
                    Excel.Range range = (Excel.Range)rangeObject;
                    object rangeValue = range.Value2;
                    if (rangeValue == null)
                    {
                        softwareCodeTable[i, j] = null;
                    }
                    else
                    {
                        softwareCodeTable[i, j] = rangeValue.ToString().Trim();
                    }
                    //result += " " + cellValue;
                }
            }
            // read rows for each products
            return softwareCodeTable;
        }

        static public string[,] LoadScriptFiles(string scriptFolderPath)
        {
            // for each script file, first get product model from scrip filename; 
            // then parse the content of the script file to get test parameters; 
            // finally verify datasheet by test parameters.
            // there are 3 modes for the datasheet items: unverified (grey) | mismatch (red) | match (green)
            foreach (string fileName in Directory.GetFiles(scriptFolderPath, "*.ini", SearchOption.AllDirectories))
            {
                ParseProductModel(fileName);
                ReadTestScript(fileName);
                VerifyDatasheet();
            }
            return softwareCodeTable;
    
        }
        
        static public bool ParseProductModel(string scriptFileName)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                string pattern = @"(?<model>[A-Za-z]+\d{4}[CM]?)(?<version>\w{4})";
                Regex _regex = new Regex(pattern);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptFileName);
                MatchCollection mc = _regex.Matches(fileNameWithoutExtension);
                productModel = string.Empty;
                productVersion = string.Empty;

                foreach (Match match in mc)
                {
                    GroupCollection gc = match.Groups;
                    productModel = gc["model"].ToString();
                    productVersion = gc["version"].ToString();
                    //strBuilder.AppendLine(fileNameWithoutExtension + "\t" + gc["model"].Value + "\t" + gc["version"]);
                }
                //TextBox t = Application.OpenForms["Form1"].Controls["textBox1"] as TextBox;
                //t.ReadOnly = false;
                //t.AppendText(strBuilder.ToString());
                //t.ReadOnly = true;

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("ParseProductModel process failed: \n" + exception.ToString());
                return false;
            }
        }

        static public bool ReadTestScript(string scriptFileName)
        {
            // reset class-wide parameters
            numberOfTestPoints = 0;
            testParams = new List<string[]>();

            string[] fileToLinesDelimiters = new[] { "\r\n" };
            string[] lineToParamsDelimiters = new[] { "\t", "," };
            string scriptFileRead;
            List<string> scriptFileLines = new List<string>();
            string[] scriptFileLineParams;

            try
            {
                // read file content including all delimiters
                StreamReader sr = new StreamReader(scriptFileName);
                scriptFileRead = sr.ReadToEnd();
                sr.Close();

                if (!scriptFileRead.Equals(string.Empty))
                {
                    scriptFileLines.AddRange(scriptFileRead.Split(fileToLinesDelimiters, StringSplitOptions.RemoveEmptyEntries));
                    int scriptFileLinesCount = scriptFileLines.Count;

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
                                string[] testParamPerLine = new string[6];
                                for (int lineParamIndex = 0; lineParamIndex < 12; lineParamIndex++)
                                {
                                    // testType, testName, testUnit, testTarget, testSpecMin, testSpecMax
                                    if (lineParamIndex == 2 && scriptFileLineParams.Length > 2) testParamPerLine[0] = scriptFileLineParams[2].Trim().ToUpper();
                                    if (lineParamIndex == 3 && scriptFileLineParams.Length > 3) testParamPerLine[1] = scriptFileLineParams[3].Trim().ToUpper();
                                    if (lineParamIndex == 4 && scriptFileLineParams.Length > 4) testParamPerLine[2] = scriptFileLineParams[4].Trim().ToUpper();
                                    if (lineParamIndex == 7 && scriptFileLineParams.Length > 7) testParamPerLine[3] = scriptFileLineParams[7].Trim().ToUpper();
                                    if (lineParamIndex == 8 && scriptFileLineParams.Length > 8) testParamPerLine[4] = scriptFileLineParams[8].Trim().ToUpper();
                                    if (lineParamIndex == 9 && scriptFileLineParams.Length > 9) testParamPerLine[5] = scriptFileLineParams[9].Trim().ToUpper();
                                }
                                testParams.Add(testParamPerLine);
                                numberOfTestPoints++;
                            }
                        } // if line starts with "'", skip it
                    } // for loop of each line scanning


                }
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("ReadTestScript process failed: \n" + exception.ToString());
                return false;
            }
        }

        static public bool VerifyDatasheet()
        {
            try
            {
                List<int> sameModelWithVariousSWRev = new List<int>();
                List<string[]> subTestParams = new List<string[]>();

                // get datasheet - softwareCodeTable items for this model
                // note the first row stores table titles, index starts from 1!
                for (int i = 1; i < softwareCodeTable.GetLength(0); i++)
                {
                    string model = softwareCodeTable[i, 0];
                    string modelWithNewFormat = model.Replace("-", string.Empty);
                    if (modelWithNewFormat.Equals(productModel))
                    {
                        // get subset for this model
                        sameModelWithVariousSWRev.Add(i);
                    }
                }
                // get the last line contains "SWREV"
                // to-do: probably need to change testParams and subTestParams back to string arrays as order matters
                int indexOfMatchedModelInSwCodeTable = -1;
                foreach (string[] strArray in testParams)
                {
                    if (strArray[1].Contains("SWREV"))
                    {
                        foreach (int swRevIndex in sameModelWithVariousSWRev)
                        {
                            if (softwareCodeTable[swRevIndex, 2].Equals(strArray[3])) { indexOfMatchedModelInSwCodeTable = swRevIndex; }
                        }
                    }
                }

                if (indexOfMatchedModelInSwCodeTable >= 0)
                {
                    // get subset of testParams, which only contains the following testNames
                    string[] testNames = new[] { "WHOAMI" };
                    foreach (string[] strArray in testParams)
                    {
                        foreach (string testName in testNames)
                        {
                            if (strArray[1].Equals(testName))
                            {
                                subTestParams.Add(strArray);
                            }
                        }
                    }

                    foreach (string[] strArray in subTestParams)
                    {
                        // check min or max value for whoami
                        if (strArray[4] != null && strArray[5] != null)
                        {
                            // softwareCodeTable, column 1 stores SW_WHOAMI
                            StringBuilder sb = new StringBuilder(softwareCodeTable[indexOfMatchedModelInSwCodeTable, 1]);
                            string hexNumber = sb.Replace("0x", string.Empty).Replace("0X", string.Empty).ToString();
                            string decNumber = Convert.ToInt32(hexNumber, 16).ToString();
                            if (strArray[4].Equals(decNumber) && strArray[5].Equals(decNumber))
                            {
                                softwareCodeTable[indexOfMatchedModelInSwCodeTable, softwareCodeTable.GetLength(1) - 1] = VerificationStatus.GREEN.ToString();
                            }
                        }
                    }

                }
                

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("VerifyDatasheet process failed: \n" + exception.ToString());
                return false;
            }
        }


    }
}
