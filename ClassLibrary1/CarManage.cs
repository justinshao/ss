using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class CarManage
    {
        public int Status { get; set; }
        public List<phoneNum> Result { get; set; }
    }
    public class phoneNum
    {
        public string LicensePlate { get; set; }
        public bool IsAutomaticPayment { get; set; }
        public string CarID { get; set; }
    }

}
