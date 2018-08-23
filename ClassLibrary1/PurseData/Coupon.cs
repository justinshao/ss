using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1.PurseData
{
   public class Coupon
    {
       public int Status { get; set; }
       public ListPrice Result { get; set; }
    }
   public class ListPrice {
       public List<ListDate> List { get; set; }
       public decimal Price { get; set; }
   }
   public class ListDate {
       public string CouponID { get; set; }
       public decimal Full { get; set; }
       public decimal Cut { get; set; }
       public string ReceiveTime { get; set; }
       public string DeadlineTime { get; set; }
       public int Status { get; set; }
       public decimal CapPrice { get; set; }
       public string CouponType { get; set; }
       public string Brife { get; set; }
       public decimal CutPrice { get; set; }
   }
}
