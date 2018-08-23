using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class Car
    {
      /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public List<Result> Result { get; set; }
    }


    public class Result
    {
        public string LicensePlate { get; set; }
        public string IsAutomaticPayment { get; set; }
        public string CarID { get; set; }
    }
   
}