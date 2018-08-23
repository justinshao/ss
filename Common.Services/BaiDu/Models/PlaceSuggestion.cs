using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class PlaceSuggestion
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<PlaceSuggestionResult> result { get; set; }
        public bool IsSuccess
        {
            get
            {
                return status == 0;
            }
        }
    }
    public class PlaceSuggestionResult
    {
        public string name { get; set; }
        public string uid { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string business { get; set; }
        public string cityid { get; set; }
        public PlaceSuggestionLocation location { get; set; }
    }
    public class PlaceSuggestionLocation
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
