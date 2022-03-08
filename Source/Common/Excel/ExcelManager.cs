using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Common.Excel
{
    public static class ExcelManager
    {
        //프로세스id 기억용.
        public static Application currentexcel = new Application();

        public static ExcelfileInfo GetExcelFileInfo(string filePath, string sheet)
        {
            ExcelfileInfo excelfileInfo = new ExcelfileInfo() { RowCount = 0, ColCount = 0 };
            if (FileManager.IsFileExist(filePath))
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Open(Filename: filePath);
                Worksheet worksheet1 = workbook.Worksheets.get_Item(sheet);

                application.Visible = false;
                Range range = worksheet1.UsedRange;

                excelfileInfo.ColCount = range.Columns.Count;
                excelfileInfo.RowCount = range.Rows.Count;

                application.Quit();
                DeleteObject(worksheet1);
                DeleteObject(workbook);
                DeleteObject(application);
            }
            else
            {
                Console.WriteLine($"경로 : {filePath} 확인바람.");
                MessageBox.Show($"경로 : {filePath} 확인바람.");
            }

            return excelfileInfo;
        }
        public static void SubstractExcelFile(string filePath, string sheet, ref object[,] array)
        {
            if (FileManager.IsFileExist(filePath))
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Open(Filename: filePath);
                Worksheet worksheet1 = workbook.Worksheets.get_Item(sheet);

                application.Visible = false;
                Range range = worksheet1.UsedRange;

                array = new object[range.Rows.Count + 1, range.Columns.Count + 1];

                for (int i = 1; i <= range.Rows.Count; ++i)
                {
                    for (int j = 1; j <= range.Columns.Count; ++j)
                    {
                        if (range.Cells[i, j] != null)
                        {
                            try
                            {
                                array[i - 1, j - 1] = (range.Cells[i, j] as Range).Value2.ToString();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"error : range.Cells[{i}, {j}] : {e}");
                                array[i - 1, j - 1] = "null";
                            }
                        }

                        else
                            array[i - 1, j - 1] = "null";
                    }
                }
                application.Quit();
                DeleteObject(worksheet1);
                DeleteObject(workbook);
                DeleteObject(application);
            }
            else
            {
                Console.WriteLine($"경로 : {filePath} 확인바람.");
                MessageBox.Show($"경로 : {filePath} 확인바람.");
            }

        }

        public static void DeleteObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
    public class ExcelfileInfo
    {
        public int ColCount { get; set; }
        public int RowCount { get; set; }
    }
}
