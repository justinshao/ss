using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Common.Utilities.WebHelper
{
    public static class PolicyTooltipHelper
    {
        /// <summary>
        /// 生成tooltip
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="labelId">标签编号</param>
        /// <param name="value">显示值</param>
        public static void MakeToolTip(GridViewRow row, string labelId, string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= 6)
            {
                return;
            }
            int times = 0;
            int length = -1;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '/')
                {
                    times++;
                    if (times == 3) { length = i; }
                    if (times == 4) { break; }
                }
            }
            if (times < 4) { return; }
            (row.FindControl(labelId) as Label).Text = value.Substring(0, length) + "..";
            (row.FindControl(labelId) as Label).ToolTip = value;
        }

        /// <summary>
        /// 生成tooltip
        /// </summary>
        /// <param name="value">显示值</param>
        /// <returns>tooltip内容</returns>
        public static string MakeToolTip(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= 6)
            {
                return value;
            }
            int times = 0;
            int length = -1;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '/')
                {
                    times++;
                    if (times == 3) { length = i; }
                    if (times == 4) { break; }
                }
            }
            if (times < 4)
            {
                return value; 
            }
            else
            {
                return value.Substring(0, length) + "..";
            }
        }
    }
}
