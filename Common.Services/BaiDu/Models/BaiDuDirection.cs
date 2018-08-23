using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuDirection
    {
        public int status { get; set; }
        public string message { get; set; }
        public int type { get; set; }
        public DirectionResult result { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class DirectionResult
    {
        public List<BaiDuRoutes> routes { get; set; }
        public BaiDuOrigin origin { get; set; }
        public BaiDuDestination destination { get; set; }
        public string traffic_condition { get; set; }
    }
    public class BaiDuRoutes
    {
        public int distance { get; set; }
        public int duration { get; set; }
        public List<BaiDuSteps> steps { get; set; }
        public int toll { get; set; }
        public BaiDuLocation originLocation { get; set; }
        public BaiDuLocation destinationLocation { get; set; }
    }
    public class BaiDuSteps
    {
        public int area { get; set; }
        public int direction { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public string instructions { get; set; }
        public string path { get; set; }
        public int turn { get; set; }
        public BaiDuLocation stepOriginLocation { get; set; }
        public BaiDuLocation stepDestinationLocation { get; set; }

    }
    public class BaiDuLocation
    {
        public string lng { get; set; }
        public string lat { get; set; }
    }
    public class BaiDuOrigin
    {
        public int area_id { get; set; }
        public string cname { get; set; }
        public string uid { get; set; }
        public string wd { get; set; }
        public BaiDuLocation originPt { get; set; }
    }
    public class BaiDuDestination
    {
        public int area_id { get; set; }
        public string cname { get; set; }
        public string uid { get; set; }
        public string wd { get; set; }
        public BaiDuLocation destinationPt { get; set; }
    }

}
