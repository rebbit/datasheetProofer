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

    class FileParser
    {

        // select tags/markers to identify the table content to be parsed
        private string[] keywordTags = { "Software Codes", "Note: Table 3", };

        public void LoadDataSheet(string datasheetFileName, out string[,] specsTable)
        {
            Excel._Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range startTag = null;
            Excel.Range endTag = null;

            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(datasheetFileName, 
                0, 
                true, 
                5, 
                "", 
                "", 
                true, 
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, 
                "\t", 
                false, 
                false, 
                0, 
                true, 
                1, 
                0);
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

            int cols = 8;
            int rows = ePos[0] - sPos[0] + 1;
            specsTable = new string[rows, cols];
//            softwareCodeTableStatus = new VerificationStatus[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    specsTable[i, j] = null;
 //                   softwareCodeTableStatus[i, j] = VerificationStatus.GRAY;  //initialize the status table

                    object rangeObject = xlWorkSheet.Cells[sPos[0] + i, sPos[1] + j];
                    Excel.Range range = (Excel.Range)rangeObject;
                    object rangeValue = range.Value2;
                    if (rangeValue != null)
                    {
                        specsTable[i, j] = rangeValue.ToString().Trim();
                        if (i == 0)
                        {
                            specsTable[i, j] = specsTable[i, j].ToUpper();
                        }
                    }
                }
            }
            // read rows for each products
        }


    }
}

