﻿using System;
using System.Collections.Generic;
using System.Web;

namespace SmartSystem.AliPayServices.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}