using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
   public class prompt
    {
        public bool result { get; set; }
        public string msg { get; set; }
        public dynamic data { get; set; }

        public static prompt Error(string msg = null, dynamic data = null)
        {
            return new prompt
            {
                result = false,
                msg = msg,
                data = data
            };
        }
        public static prompt Success(string msg = null, dynamic data = null)
        {
            return new prompt
            {
                result = true,
                msg = msg,
                data = data
            };

        }
    }
}
