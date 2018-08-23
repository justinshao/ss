using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuUserLocation
    {
        public int status { get; set; }
        public string address { get; set; }
        public LocationContent content { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class LocationContent
    {
        public string address { get; set; }
        public AddressDetail address_detail { get; set; }
        public CurrPoint point { get; set; }
    }
    public class AddressDetail
    {
        public string city { get; set; }
        public int city_code { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
    }
    public class CurrPoint
    {
        public string x { get; set; }
        public string y { get; set; }
    }
}
