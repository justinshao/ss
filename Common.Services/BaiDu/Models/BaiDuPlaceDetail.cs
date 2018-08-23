using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuPlaceDetail
    {
        public int status { get; set; }

        public string message { get; set; }
        public PlaceDetaiResult result { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class PlaceDetaiResult
    {
        public string name { get; set; }
        public PlaceDetaiLocation location { get; set; }
        public string address { get; set; }
        public int detail { get; set; }
        public string uid { get; set; }
        public string street_id { get; set; }
        public PlaceDetaiInfo detail_info { get; set; }

    }
    public class PlaceDetaiLocation
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
    public class PlaceDetaiInfo
    {
        public string tag { get; set; }
        public string detail_url { get; set; }
        public string type { get; set; }
        public string overall_rating { get; set; }
        public string service_rating { get; set; }
        public string environment_rating { get; set; }
        public string image_num { get; set; }
        public string comment_num { get; set; }
    }
}
