using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace DatasheetProofer
{
    class ParseExcel
    {
        static public string LoadDataSheet(string datasheetFileName)
        {
            Excel._Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range firstFind = null;
            Excel.Range currentFind = null;

            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(datasheetFileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // set the find range as col A
            Excel.Range tableTitles = (Excel.Range)xlWorkSheet.Columns["A:B", Type.Missing];

            // search for "software code" table
            // detailed datasheet will be considered
            currentFind = tableTitles.Find("Software Codes",
                xlWorkSheet.Cells[1, 1],
                Excel.XlFindLookIn.xlValues,
                Excel.XlLookAt.xlPart,
                Excel.XlSearchOrder.xlByRows,
                Excel.XlSearchDirection.xlNext,
                false, false, false);
            while (currentFind != null)
            {
                if (firstFind == null)
                {
                    firstFind = currentFind;
                }
                else if (currentFind.get_Address(false, false, Excel.XlReferenceStyle.xlA1, false, false) == firstFind.get_Address(false, false, Excel.XlReferenceStyle.xlA1, false, false))
                {
                    break;
                }
                currentFind = tableTitles.FindNext(currentFind);
            }

            //get the right position for the software codes table
            string sAddress = currentFind.get_Address(false, false, Excel.XlReferenceStyle.xlA1, false, false);
            // read rows for each products
            return sAddress;

        }
    }
}
