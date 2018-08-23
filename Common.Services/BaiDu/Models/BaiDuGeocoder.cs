using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuGeocoder
    {
        public int status { get; set; }
        public GeocoderResult result { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class GeocoderResult
    {
        public AddressComponent addressComponent { get; set; }
    }
    public class AddressComponent
    {
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
    }
}
