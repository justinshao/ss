using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinInerface.WXService;

namespace SmartSystem.WeiXinInerface
{
    public class AdminOperateServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="parkingId">车场编号</param>
        /// <param name="gateId">通道编号</param>
        /// <param name="remark">备注</param>
        /// <returns>0-成功 1-车场网络异常 2-通道不支持远程开门 3-开闸失败</returns>
        public static int RemoteGate(string userId,string parkingId,string gateId,string remark) {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.WXRemoteGate(userId,parkingId, gateId, remark);
            client.Close();
            client.Abort();
            return result;
        }
    }
}
