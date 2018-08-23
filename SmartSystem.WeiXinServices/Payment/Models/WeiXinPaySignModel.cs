using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinServices.Payment
{
    public class WeiXinPaySignModel
    {
        public string AppId { get; set; }
        public string Package { get; set; }
        public string Timestamp { get; set; }
        public string Noncestr { get; set; }
        private string _SignType;
        public string SignType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SignType))
                    return "MD5";
                return _SignType;
            }
            set
            {
                _SignType = value;
            }
        }

        public string PaySign { get; set; }
    }
}
