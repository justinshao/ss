using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZXAPI
{
    public class OutParkingModel
    {
        public string app_id { set; get; }
        public string armCode { set; get; }
        public string outAramName { set; get; }
        public string calcCharge { set; get; }
        public string carNumber { set; get; }
        public string chargeItemCnt { set; get; }
        public List<ChargeItem> chargeItems { set; get; }
        public string inTime { set; get; }
        public string outTime { set; get; }
        public string parkCode { set; get; }
        public string parkingDuration { set; get; }
        public string recordId { set; get; }
        public string salt { set; get; }
        public string sign { set; get; }
        public string sign_type { set; get; }
        public string totalCharge { set; get; }
        public string vplOwner { set; get; }
        public string vplType { set; get; }
        public string payedCharge { set; get; }
        public string parkType { set; get; }
    }

    public class ChargeItem
    {
        public string charger { set; get; }
        public string seqNum { set; get; }
        public string chargeType { set; get; }
        public string time { set; get; }
        public string account { set; get; }

    }
}
