using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using SmartSystem.WeiXinBase;

namespace SmartSystem.WeiXinInteraction.ForKeywork
{
    public class ReplyText : IKey
    {
        public IWRespBase ReplyContent(WX_ApiConfig config,WX_Keyword gKey, WReqBase request, string qValue)
        {
            var response = request.CreateResponse<WRespText>();
            response.Content = gKey.Text;
            return response;
        }
    }
}
