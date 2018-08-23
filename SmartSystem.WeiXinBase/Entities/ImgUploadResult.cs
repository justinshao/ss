using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    public class ImgUploadResult
    { /// <summary>
      /// 错误码
      /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }
        public string url { get; set; }
    }
}
