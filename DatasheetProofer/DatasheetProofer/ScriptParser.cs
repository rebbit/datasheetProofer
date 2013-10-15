using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    class ScriptParser
    {
        private string productModel;
        private string productVersion;
        private int numberOfTestPoints = 0;
        private List<string[]> testParams = new List<string[]>();

        public void LoadScriptFiles(string scriptFolderPath, string[,] specsTable, out VerificationStatus[,] specsTableStatus)
        {
            specsTableStatus = new VerificationStatus[,] { };
            // for each script file, first get product model from scrip filename; 
            // then parse the content of the script file to get test parameters; 
            // finally verify datasheet by test parameters.
            // there are 3 modes for the datasheet items: unverified (grey) | mismatch (red) | match (green)
            foreach (string fileName in Directory.GetFiles(scriptFolderPath, "*.ini", SearchOption.AllDirectories))
            {
                ParseProductModel(fileName);
                ReadTestScript(fileName);
                VerifyDatasheet(specsTable, out specsTableStatus);
            }
        }

        public bool ParseProductModel(string scriptFileName)
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

        public bool ReadTestScript(string scriptFileName)
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
                        // Current "rules" for commenting in the script file:
                        //   - line starting with "'" usually comments itself
                        //   - multiple comments usually start with "*"
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

        public void VerifyDatasheet(string[,] specsTable, out VerificationStatus[,] specsTableStatus)
        {
                int rows = specsTable.GetLength(0);
                int cols = specsTable.GetLength(1);
                specsTableStatus = new VerificationStatus[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        specsTableStatus[i, j] = VerificationStatus.GRAY;
                    }
                }

                List<int> sameModelWithVariousSWRev = new List<int>();
                List<string[]> subTestParams = new List<string[]>();

                // get datasheet - softwareCodeTable items for this model
                // note the first row stores table titles, index starts from 1!
                for (int i = 1; i < specsTable.GetLength(0); i++)
                {
                    string model = specsTable[i, 0];
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
                            if (specsTable[swRevIndex, 2].Equals(strArray[3])) { indexOfMatchedModelInSwCodeTable = swRevIndex; }
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
                            StringBuilder sb = new StringBuilder(specsTable[indexOfMatchedModelInSwCodeTable, 1]);
                            string hexNumber = sb.Replace("0x", string.Empty).Replace("0X", string.Empty).ToString();
                            string decNumber = Convert.ToInt32(hexNumber, 16).ToString();
                            if (strArray[4].Equals(decNumber) && strArray[5].Equals(decNumber))
                            {
                                if (specsTableStatus[indexOfMatchedModelInSwCodeTable, 1] == VerificationStatus.GRAY)
                                {
                                    specsTableStatus[indexOfMatchedModelInSwCodeTable, 1] = VerificationStatus.GREEN;
                                }
                            }
                            else // may need more robust check
                            {
                                specsTableStatus[indexOfMatchedModelInSwCodeTable, 1] = VerificationStatus.RED;
                            }
                        }
                    }

                }
        }

    }
}
