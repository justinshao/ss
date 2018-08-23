using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuRoutematrix
    {
        public int status { get; set; }
        public string message { get; set; }
        public RouteResult result { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class RouteResult
    {
        public List<RouteElements> elements { get; set; }
    }
    public class RouteElements
    {
        public RouteValue distance { get; set; }
        public RouteValue duration { get; set; }
    }
    public class RouteValue
    {
        public string text { get; set; }
        public string value { get; set; }
    }
}