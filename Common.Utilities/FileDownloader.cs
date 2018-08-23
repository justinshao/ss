namespace Common.Utilities
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    public class FileDownloader
    {
        public static void TransferFile(string localFileName, string downloadFileName)
        {
            if (string.IsNullOrEmpty(localFileName)) throw new ArgumentNullException("localFileName");
            if (string.IsNullOrEmpty(downloadFileName)) throw new ArgumentNullException("downloadFileName");
            HttpResponse response = HttpContext.Current.Response;

            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(downloadFileName) + "\";");
            response.WriteFile(localFileName);
            response.Flush();
            response.End();
        }

        public static void ExportToExcel(DataTable data, string fileName, string title)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if (data != null && data.Rows.Count > 0)
            {
                var sbContents = BuildContent(data, title);
                ExportToExcel(sbContents, fileName);
            }
        }

        public static void ExportToExcel(string data, string fileName)
        {
            
            HttpResponse response = HttpContext.Current.Response;
            response.Buffer = true;
            response.ClearContent();
            response.ClearHeaders();
            response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html\" charset=\"utf-8\">");
            AddResponseFileName(fileName + ".xls", response);
            response.ContentType = "application/vnd.ms-excel";
            response.Write(data);
            response.Flush();
            response.End();
        }

        public static void AddResponseFileName(string fileName, HttpResponse response)
        {
            string downloadFileName = fileName;
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            {
                downloadFileName = HttpUtility.UrlPathEncode(fileName);
            }
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + downloadFileName  + "\"");
            }
            else
            {
                response.AddHeader("Content-Disposition", "attachment;filename=" + downloadFileName);
            }
        }

        public static string BuildContent(DataTable data, string title)
        {
            int colsCount = data.Columns.Count;
            var sbContents = new StringBuilder();
            sbContents.Append(
                "<div><table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-style:Solid;font-family:Verdana;border-collapse:collapse;\">");
            if (!string.IsNullOrEmpty(title))
            {
                sbContents.AppendFormat(
                    "<tr><td colspan={0} align=\"center\" style=\"color:Black;font-size:10pt;font-weight:bold;\">{1}</td></tr>",
                    colsCount, title);
            }
            sbContents.Append("<tr align=\"center\" style=\"color:Black;font-size:10pt;font-weight:bold;\">");
            for (int colIndex = 0; colIndex < colsCount; colIndex++)
            {
                sbContents.AppendFormat("<td align=\"center\" style=\"background-color:#00FFFF\">{0}</td>",
                                        data.Columns[colIndex].ColumnName);
            }
            sbContents.Append("</tr>");
            foreach (DataRow row in data.Rows)
            {
                sbContents.Append("<tr>");
                for (int colIndex = 0; colIndex < colsCount; colIndex++)
                {
                    sbContents.Append("<td align=\"center\">");
                    object cellContent = row[colIndex];
                    if (cellContent != null)
                    {
                        sbContents.Append(cellContent.ToString());
                    }
                    sbContents.Append("</td>");
                }
                sbContents.Append("</tr>");
            }
            sbContents.Append("</table></div>");
            return sbContents.ToString();
        }

        public static void ExportFile(DataTable dt, string fileName, List<string> columnNames, int[] mergeCondition, int[] noMergeColumns)
        {
             ExportFile(dt, fileName, columnNames, mergeCondition, noMergeColumns, new int[] { },new int[]{});
        }
        public static void ExportFile(DataTable dt, string fileName, List<string> columnNames, int[] mergeCondition, int[] noMergeColumns, int[] wrapColumns,int [] numerberColumns)
        {
            using (var book = new NPOI.HSSF.UserModel.HSSFWorkbook())
            {
                var sheet = book.CreateSheet();
                var headRow = sheet.CreateRow(0);
               
                for (int i = 0; i < columnNames.Count; i++)
                {
                    var cell = headRow.CreateCell(i);
                    cell.SetCellValue(columnNames[i]);
                    var font=book.CreateFont();
                    font.Boldweight = NPOI.HSSF.UserModel.HSSFFont.BOLDWEIGHT_BOLD;
                    cell.RichStringCellValue.ApplyFont(font);
                }
                foreach (var wrapC in wrapColumns)
                {
                    NPOI.HSSF.UserModel.HSSFCellStyle cs = book.CreateCellStyle();
                    cs.WrapText = true;
                    sheet.SetDefaultColumnStyle((short)wrapC, cs);
                }
                //开始写入内容
                int RowCount = dt.Rows.Count;//行数
                int ColCount = columnNames.Count;//列数
                for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                {
                    var row = sheet.CreateRow(rowIndex + 1);
                    for (int colIndex = 0; colIndex < ColCount; colIndex++)
                    {
                        var cell = row.CreateCell(colIndex);
                        if (IsWrapColumn(wrapColumns, colIndex))
                        {
                            cell.SetCellValue(dt.Rows[rowIndex][colIndex].ToString().Replace("</br>", "\n"));
                        }
                        else
                        {
                            cell.SetCellValue(dt.Rows[rowIndex][colIndex].ToString());
                        }                        
                    }
                }
                for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < ColCount; colIndex++)
                    {
                        if (noMergeColumns.Contains(colIndex)) continue;
                        var rowSpan = GetRowSpan(dt, mergeCondition, rowIndex, colIndex);
                        if (rowSpan <= 1) continue;
                        sheet.AddMergedRegion(new NPOI.HSSF.Util.CellRangeAddress(rowIndex + 1, rowIndex + rowSpan, colIndex, colIndex));
                    }
                }
                ////设置每列的宽度
                //for (int colIndex = 0; colIndex < columnNames.Count; colIndex++)
                //{
                //    sheet.AutoSizeColumn(colIndex);
                //    sheet.SetColumnWidth(colIndex, sheet.GetColumnWidth(colIndex) + 2100);
                //}
                ExportFile(book, fileName);
            }
        }
        private static bool IsWrapColumn(int [] wrapColumns, int i ) {
            if (wrapColumns == null || wrapColumns.Count() == 0) return false;
            return  wrapColumns.Where(r => r == i).Count() > 0;
        }
        public static void ExportFile(NPOI.HSSF.UserModel.HSSFWorkbook book, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                var response = HttpContext.Current.Response;
                book.Write(stream);
                response.Clear();                             //清除缓冲区流中的所有内容输出
                response.ClearHeaders();                      //清除缓冲区流中的所有头，不知道为什么，不写这句会显示错误页面
                response.Buffer = false;                      //设置缓冲输出为false
                //设置输出流的 HTTP MIME 类型为application/octet-stream
                response.ContentType = "application/octet-stream";
                AddResponseFileName(fileName+".xls", response);
                response.AppendHeader("Content-Length", stream.Length.ToString());
                //将指定的文件直接写入 HTTP 内容输出流。
                //一定要注意是WriteFile不是Write（害得我搞了一晚上）
                response.BinaryWrite(stream.ToArray());
                response.Flush();        //向客户端发送当前所有缓冲的输出
                response.End();          //将当前所有缓冲的输出发送到客户端，这句户有时候会出错，可以尝试把这句话放在整个函数的最后一行。也可以用HttpContext.Current.ApplicationInstance.CompleteRequest ()方法代替
            }
        }

        private static int GetRowSpan(DataTable dt, int[] mergeCondition, int rowIndex, int colIndex)
        {
            if (rowIndex > 0)//第一行不与前一行比较
            {
                var currRow = dt.Rows[rowIndex];
                var preRow = dt.Rows[rowIndex - 1];
                if (SameText(currRow, preRow, colIndex, mergeCondition))//与前一行内容相等，隐藏
                {
                    return 0;
                }
            }
            return GetSameRowCount(dt, mergeCondition, rowIndex, colIndex);//与前一行内容不等，所占行数为与后面行内容相等的行数
        }

        private static int GetSameRowCount(DataTable dt, int[] mergeCondition, int rowIndex, int colIndex)
        {
            var result = 1;
            var currRow = dt.Rows[rowIndex];
            for (var i = rowIndex + 1; i < dt.Rows.Count; i++)
            {
                var nextRow = dt.Rows[i];
                if (SameText(nextRow, currRow, colIndex, mergeCondition))//第n+i行与第n行内容相等
                    result++;
                else//第n+i行与第n行内容不等
                    return result;
            }
            return result;
        }

        private static bool SameText(DataRow currRow, DataRow preRow, int colIndex, int[] condition)
        {
            return currRow[colIndex].ToString() == preRow[colIndex].ToString() && condition.All(p => currRow[p].ToString() == preRow[p].ToString());
        }

        public static void ExportList<T>(IEnumerable<T> dataList,string fileName, string[] columnNames, 
            Action<T,Action<object>> writer)
        {
            using (var book = new NPOI.HSSF.UserModel.HSSFWorkbook())
            {
                var sheet = book.CreateSheet();
                var headRow = sheet.CreateRow(0);

                for (int i = 0; i < columnNames.Length; i++)
                {
                    var cell = headRow.CreateCell(i);
                    cell.SetCellValue(columnNames[i]);
                    var font = book.CreateFont();
                    font.Boldweight = NPOI.HSSF.UserModel.HSSFFont.BOLDWEIGHT_BOLD;
                    cell.RichStringCellValue.ApplyFont(font);
                }
                //开始写入内容
                var rowIndex = 1;
                foreach(T item in dataList)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    var colIndex = 0;
                    writer(item, (target) =>
                    {
                        row.CreateCell(colIndex++).SetCellValue(target == null ? "" : target.ToString());
                    });
                }
                //设置每列的宽度
                for (int colIndex = 0; colIndex < columnNames.Length; colIndex++)
                {
                    sheet.AutoSizeColumn(colIndex);
                    sheet.SetColumnWidth(colIndex, sheet.GetColumnWidth(colIndex) + 2100);
                }
                ExportFile(book, fileName);
            }
        }
    }
}
