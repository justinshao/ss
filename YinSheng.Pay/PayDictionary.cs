using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace YinSheng.Pay
{
    /// <summary>
    /// 支付
    /// 扩展键值对类
    /// </summary>
    public class PayDictionary : Dictionary<string, string>
    {
        public PayDictionary()
        {

        }

        /// <summary>
        ///当前排序  
        /// </summary>
        public void Sort(PaySortEnum pse)
        {
            List<string> listKeys = this.Keys.ToList();
            listKeys.Sort(new SortCamparer(pse));
            PayDictionary pd = new PayDictionary();
            foreach (string r in listKeys)
            {
                pd.Add(r, this[r]);
            }
            this.Clear();
            foreach (string r in listKeys)
            {
                this.Add(r, pd[r]);
            }
            pd.Clear();
            pd = null;
        }

        /// <summary>
        /// 按keys值排序，排除keys之外的
        /// </summary>
        public PayDictionary Sort(string[] keys)
        {
            List<string> listKeys = keys.ToList();
            PayDictionary pd = new PayDictionary();
            foreach (string r in listKeys)
            {
                pd.Add(r, this[r]);
            }
            return pd;
        }

        /// <summary>
        /// 获取参数形式字符串
        /// </summary>
        /// <returns></returns>
        public string GetParmarStr()
        {
            StringBuilder result = new StringBuilder();
            this.Aggregate(result, (s, b) => s.Append(b.Key + "=" + b.Value + "&"));
            return result.ToString().TrimEnd('&');
        }

        /// <summary>
        /// http参数字符串赋值
        /// </summary>
        public void SetParmarStr(string par)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(par);
            string[] parNames = nvc.AllKeys.OrderBy(m => m).ToArray();
            for (int i = 0; i < parNames.Length; i++)
            {
                this.Add(parNames[i], nvc[parNames[i]]);
            }
            nvc.Clear();
            nvc = null;
        }

        /// <summary>
        /// 转换成HTML页面POST提交字符
        /// </summary>
        public string TransFormHTML(string Url)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<html>");
            html.Append("<head>");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            html.Append("</head>");
            html.Append("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", Url);
            foreach (KeyValuePair<string, string> row in this)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", row.Key, row.Value);
            }
            html.Append("</form>");
            html.Append("<script type=\"text/javascript\">");
            html.Append("function OnLoadSubmit()");
            html.Append("{");
            html.Append("document.getElementById(\"pay_form\").submit();");
            html.Append("};");
            html.Append("</script>");
            html.Append("</body>");
            html.Append("</html>");
            return html.ToString();
        }
    }
    public enum PaySortEnum
    {
        /// <summary>
        /// 降序
        /// </summary>
        Asc = 0,
        /// <summary>
        /// 升序
        /// </summary>
        Desc = 1
    }

    /// <summary>
    /// 比較器，用於排序
    /// </summary>
    public class SortCamparer : IComparer<string>
    {
        private PaySortEnum pse { get; set; }
        public SortCamparer(PaySortEnum p)
        {
            pse = p;
        }

        public int Compare(string obj1, string obj2)
        {
            if (pse == PaySortEnum.Asc)
            {
                if (string.Compare(obj1, obj2, StringComparison.Ordinal) > 0)
                    return 1;
                else if (string.Compare(obj1, obj2, StringComparison.Ordinal) < 0)
                    return -1;
            }
            else
            {
                if (string.Compare(obj1, obj2, StringComparison.Ordinal) > 0)
                    return -1;
                else if (string.Compare(obj1, obj2, StringComparison.Ordinal) < 0)
                    return 1;
            }
            return 0;
        }
    }

}
