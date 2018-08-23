using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;

namespace Common.Entities.WX
{
    public class ConsumerDiscountResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Result { get; set; }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkCarDerate ResModel { set; get; }
        public List<ParkDerateQRcode> QRCodeResult { get; set; }
        public List<ParkCarDerate> CarDerates { set; get; }
        public string ParkingID { get; set; }
        public BaseParkinfo Parking { get; set; }
    }
}
