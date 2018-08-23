using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Common.Utilities
{
    public static class ReportOperator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="conditionColunm">存储的是需要判断是否重复求和的</param>
        /// <param name="SumColunm">需要求和的列</param>
        /// <param name="ExceptColunm">存储的是不需要判断是否重复加减的</param>
        /// <returns></returns>
        public static DataTable SumDataTable(DataTable table, int[] conditionColunm, int[] SumColunm, int[] ExceptColunm)
        {
            return SumDataTable(table, conditionColunm, SumColunm, ExceptColunm, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="conditionColunm">存储的是需要判断是否重复求和的</param>
        /// <param name="SumColunm">需要求和的列</param>
        /// <param name="ExceptColunm">存储的是不需要判断是否重复加减的</param>
        /// <returns></returns>
        public static DataTable SumDataTable(DataTable table, int[] conditionColunm, int[] SumColunm, int[] ExceptColunm, bool isIncludeSelf)
        {
            Dictionary<int, int> DicExceptColunm = new Dictionary<int, int>();
            foreach (int i in ExceptColunm)
            {
                if (!DicExceptColunm.ContainsKey(i))
                {
                    DicExceptColunm.Add(i, i);
                }
            }
            DataRow TotalDr = table.NewRow();
            decimal[] TotalColunm = new decimal[SumColunm.Count()];

            for (int g = 0; g < SumColunm.Count(); g++)
            {
                TotalColunm[g] = 0;
            }

            for (int k = 0; k < table.Columns.Count; k++)
            {
                TotalDr[k] = table.Rows[0][k];
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                int index = 0;
                foreach (int m in SumColunm)
                {
                    if (i > 0)
                    {
                        DataRow preDr = table.Rows[i - 1];
                        DataRow nowDr = table.Rows[i];
                        if (!DicExceptColunm.ContainsKey(m))
                        {
                            int[] con = conditionColunm;
                            if (isIncludeSelf)
                            {
                                var condition = conditionColunm.Where(r => true).ToList();
                                condition.Add(m);
                                con = condition.ToArray();
                            }
                            if (IsSame(preDr, nowDr, con))
                            {
                                index++;
                                continue;
                            }
                        }
                    }
                    TotalColunm[index] += decimal.Parse(CheckIsNoNull(table.Rows[i][m].ToString()));
                    index++;
                }
            }

            int Sindex = 0;
            foreach (int m in SumColunm)
            {
                TotalDr[m] = TotalColunm[Sindex].ToString();
                Sindex++;
            }
            table.Rows.Add(TotalDr);
            return table;
        }

        public static DataTable AvgDataTable(DataTable table, int[] conditionColunms, int[] avgColumns)
        {
            var lastRow = table.Rows[table.Rows.Count - 1];
            foreach (var item in avgColumns)
            {
                lastRow[item] = FormatLength(table.Rows.Count - 1, lastRow, item);
            }
            return table;
        }

        private static string FormatLength(int distinctCount, DataRow lastRow, int item)
        {
            var result = Math.Round(decimal.Parse(lastRow[item].ToString()) / distinctCount, 2);
            return string.Format("{0:00.00}", result);
        }

        /// <summary>
        /// 为空则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string CheckIsNoNull(string value)
        {
            return string.IsNullOrEmpty(value.Trim()) ? "0" : value.Trim();
        }

        //判断前一行数据的指定列是否有重复数据
        private static bool IsSame(DataRow preDr, DataRow nowDr, int[] condition)
        {
            bool IsSame = true;
            foreach (int i in condition)
            {
                if (preDr[i].ToString().Trim() != nowDr[i].ToString().Trim())
                {

                    IsSame = false;
                    break;
                }
            }
            return IsSame;
        }

        public static System.Web.UI.WebControls.GridView ClearLastRowsColumn(System.Web.UI.WebControls.GridView gv, int[] NotClearColumns)
        {
            Dictionary<int, int> DicNotClearColumn = new Dictionary<int, int>();
            foreach (int i in NotClearColumns)
            {
                if (!DicNotClearColumn.ContainsKey(i))
                {
                    DicNotClearColumn.Add(i, i);
                }
            }
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (DicNotClearColumn.ContainsKey(i))
                {
                    continue;
                }
                gv.Rows[gv.Rows.Count - 1].Cells[i].Text = "";
            }
            return gv;
        }

        public static void ClearLastRowsColumn(DataTable dt, int[] NotClearColumns)
        {
            Dictionary<int, int> DicNotClearColumn = new Dictionary<int, int>();
            foreach (int i in NotClearColumns)
            {
                if (!DicNotClearColumn.ContainsKey(i))
                {
                    DicNotClearColumn.Add(i, i);
                }
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (DicNotClearColumn.ContainsKey(i))
                {
                    continue;
                }
                dt.Rows[dt.Rows.Count - 1][i] = "";
            }
        }

        private static DataRow ClearCell(DataRow dr, DataTable table, int[] sumColumn)
        {
            Dictionary<int, int> DicSumColunm = new Dictionary<int, int>();
            foreach (int index in sumColumn)
            {
                if (!DicSumColunm.ContainsKey(index))
                {
                    DicSumColunm.Add(index, index);
                }
            }
            int columCount = table.Columns.Count;
            for (int i = 0; i < columCount; i++)
            {
                if (!DicSumColunm.ContainsKey(i))
                {
                    dr[i] = dr[i].GetType();
                }
            }
            return dr;
        }

        public static void ExportToExcel(Page page, HttpResponse response, GridView gv, int[] noneClearCloumns, int[] numberFormatColumns, string fileName)
        {
            gv.HeaderRow.Style.Add("background-color", "#00FFFF");
            gv.Style.Add("cellspacing", "0");
            gv.Style.Add("rules", "all");
            gv.Style.Add("border", "1");
            gv.Style.Add("border-style", "Solid");
            gv.Style.Add("font-family", "Verdana");
            gv.Style.Add("border-collapse", "collapse");
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                foreach (var item in numberFormatColumns)
                {
                    gv.Rows[i].Cells[item].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
            gv = ReportOperator.ClearLastRowsColumn(gv, noneClearCloumns);
            response.Buffer = true;
            response.ClearContent();
            response.ClearHeaders();
            response.AddHeader("content-disposition",
                "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName) + ".xls");
            response.ContentType = "application/vnd.ms-excel";
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //建立假HtmlForm避免以下錯誤
                    //Control 'GridView1' of type 'GridView' must be placed inside 
                    //a form tag with runat=server. 
                    //另一種做法是override VerifyRenderingInServerForm後不做任何事
                    //這樣就可以直接GridView1.RenderControl(htw);
                    HtmlForm hf = new HtmlForm();
                    page.Controls.Add(hf);
                    hf.Controls.Add(gv);
                    hf.RenderControl(htw);
                    response.Write(sw.ToString());
                }
            }
        }

        public static DataTable CountDataTable(DataTable table, int[] countColumns)
        {
            var lastRow = table.Rows[table.Rows.Count - 1];
            foreach (var item in countColumns)
            {
                lastRow[item] = table.Rows.Count - 1;
            }
            return table;
        }
    }
}
