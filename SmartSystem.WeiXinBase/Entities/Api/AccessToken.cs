﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// access_token请求后的JSON返回格式
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string Accesstoken { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}
