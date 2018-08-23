using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class LockCarResult
    {
        /// <summary>
        /// 新增锁车记录ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID { set; get; }

        /// <summary>
        /// 锁车结果
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool reuslt { set; get; }
    }
}
