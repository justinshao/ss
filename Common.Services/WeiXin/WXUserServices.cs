using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.WeiXin
{
    public class WXUserServices
    {
        /// <summary>
        /// 获取关注的用户信息
        /// </summary>
        /// <param name="sPlateNo">用户绑定的车辆</param>
        /// <returns></returns>
        public static WX_Info GetWXInfoByPlateNo(string sPlateNo)
        {
            IWXUser factory = WXUserFactory.GetFactory();
            return factory.GetWXInfoByPlateNo(sPlateNo);
        }
    }
}
