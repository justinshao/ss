using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Common.Utilities.WebHelper
{
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 遍历枚举类型的所有元素
        /// </summary>
        /// <typeparam name="TEnum">枚举</typeparam>
        /// <param name="action">回调方法</param>
        public static void EnumForeach<TEnum>(Action<TEnum> action) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException("TEnum只能是枚举"); }
            foreach (var v in Enum.GetValues(typeof(TEnum)))
            {
                action((TEnum)v);
            }
        }

        /// <summary>
        /// 遍历枚举类型的所有元素的值和描述
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="action">回调方法Action(描述,值)</param>
        public static void EnumDescriptionForeach<TEnum>(Action<string, string> action) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException("类型只能是枚举"); }
            foreach (var v in Enum.GetValues(typeof(TEnum)))
            {
                action(((Enum)v).GetDescription(), ((int)v).ToString());
            }
        }

        /// <summary>
        /// 通过反射将枚举绑定到DropDownList上，withAll代表加入“所有”选项，“所有”项的值为-1
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="dropDownList"></param>
        /// <param name="withAll"></param>
        public static void BindEnum<TEnum>(this ListControl dropDownList, bool withAll) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("类型只能是枚举");
            EnumDescriptionForeach<TEnum>(dropDownList.AddItem);
            if (withAll) dropDownList.Items.Insert(0, new ListItem("所有", "-1"));
        }

        /// <summary>
        /// 增加项，为了利用EnumDescriptionForeach方法而加此扩展
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public static void AddItem(this ListControl dropDownList, string text, string value) {
            dropDownList.Items.Add(new ListItem(text, value));
        }
    }
}
