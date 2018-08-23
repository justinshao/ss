using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class ParkingList
    {
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result2 Result { get; set; }
    }
    public class Result2
    {
        /// <summary>
        /// 
        /// </summary>
        public string IsNext { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> List { get; set; }
    }
    public class ListItem
    {
        /// <summary>
        /// 停车场
        /// </summary>
        public string ParkingName { get; set; }
        /// <summary>
        /// 车牌
        /// </summary>
        public string LicensePlate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LockNo { get; set; }
    }

    public class PresenceOrderList
    {
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PresenceOrder> Result { get; set; }
    }


    public class PresenceOrder
    {
        public string LicensePlate { get; set; }
        public long Time { get; set; }
        public string CreateTime { get; set; }
        public string ParkName { get; set; }
        public string OrderNo { get; set; }
        public string OrderID { get; set; }
        public decimal Price { get; set; }

        public string TimeString
        {
            get
            {
                long iHour = Time / (60 * 60);
                long iHourY = Time % (60 * 60);
                long iMin = iHourY / 60;
                long iSec = iHourY % 60;

                return iHour.ToString("00") + ":" + iMin.ToString("00") + ":" +  iSec.ToString("00");


            }
        }
    }
}
