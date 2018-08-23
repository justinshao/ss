using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QueryparkoutAttributesRes 
    {
        public string parkCode { set; get; }
        public string parkName { set; get; }
        public string carNo { set; get; }
        public string cardNo { set; get; }
        public string cardType { set; get; }
        public string outTime { set; get; }
        public string outEventType { set; get; }
        public string outEquip { set; get; }
        public string outOperator { set; get; }
        public string payTypeName { set; get; }
        public float ysMoney { set; get; }
        public float yhMoney { set; get; }
        public float hgMoney { set; get; }
        public float ssMoney { set; get; }
        public int parkingTime { set; get; }
        public string outPhotoUrlIds { set; get; }
        public string inTime { set; get; }
        public string inEquip { set; get; }
    }
}
