using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class SystemOperatePurview
    {
        private string _id;
        public string id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_id))
                    return _id;

                switch (text)
                {
                    case "添加":
                        {
                            return "btnadd";
                        }
                    case "修改":
                        {
                            return "btnupdate";
                        }
                    case "删除":
                        {
                            return "btndelete";
                        }
                    case "刷新":
                        {
                            return "btnrefresh";
                        }
                    default: return string.Format("btntoolbart{0}", new Random(sort).Next(100, 999));
                }
            }
            set
            {
                _id = value;
            }
        }
        public int sort { get; set; }
        public string text { get; set; }
        private string _iconCls { get; set; }
        public string iconCls
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_iconCls))
                    return _iconCls;

                switch (text)
                {
                    case "添加":
                        {
                            return "icon-add";
                        }
                    case "修改":
                        {
                            return "icon-edit";
                        }
                    case "删除":
                        {
                            return "icon-remove";
                        }
                    case "刷新":
                        {
                            return "icon-reload";
                        }
                    default: return string.Empty;
                }
            }
            set
            {
                _iconCls = value;
            }
        }
        public string handler { get; set; }
    }
}