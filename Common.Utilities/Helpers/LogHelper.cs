
using Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities.Helpers
{
    public class LogerHelper
    {
        private static Logger _Loger = LogManager.GetLogger("基础日志");
        /// <summary>
        /// 日志记录对象
        /// </summary>
        public static Logger Loger
        {
            get
            {
                return _Loger;
            }
        }
    }
}
