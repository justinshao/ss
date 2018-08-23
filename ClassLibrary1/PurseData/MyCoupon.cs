using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1.PurseData
{
   public class MyCoupon
    {
       public int Status { get; set; }
       public pagelist Result { get; set; }
    }
   public class pagelist {
       public int Page { get; set; }
       public bool IsNext { get; set; }
       public List<listdata> List { get; set; }

   }
   public class listdata {
       public decimal Full { get; set; }
       public decimal Cut { get; set; }
       public decimal CapPrice { get; set; }
       public int UnitType { get; set; }
       public int Type { get; set; }
       public string CouponType { get; set; }
       public string ReceiveTime { get; set; }
       public string DeadlineTime { get; set; }
       public int Status { get; set; }
       public string Brife { get; set; }
       public string ActivityName { get; set; }
       public int ActivityType { get; set; }

   }
   
     
}
