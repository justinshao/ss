using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
   public class tgOperatePurview
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
                   case "增加":
                       return "btnadd";
                   case "修改":
                       return "btnupdate";
                   case "删除":
                       return "btndelete";
                   case "刷新":
                       return "btnrerash";
                   case "二维码下载":
                       return "btndownload";
                   default: return string.Format("btntoolbart{0}", new Random(sort).Next(100, 900));
               }
           }
           set { _id = value; }

       }
       public int sort { get; set; }
       public string text { get; set; }
       private string _iconcls;
       public string iconCls
       {
           get
           {
               if (!string.IsNullOrWhiteSpace(_iconcls))
                   return _iconcls;
               switch (text)
               {
                   case "增加":
                       return "icon-add";
                   case "修改":
                       return "icon-edit";
                   case "删除":
                       return "icon-remove";
                   case "刷新":
                       return "icon-reload";
                   case "下载二维码":
                       return "icon-print";
                   default: return string.Empty;
               }
           }
           set
           {
               _iconcls = value;
           }

       }
       public string handler { get; set; }
   }
}
