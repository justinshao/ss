using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinBase;
using Common.Entities.WX;

namespace SmartSystem.WeiXinInteraction.ForKeywork
{
    public interface IKey
    {
        IWRespBase ReplyContent(WX_ApiConfig config,WX_Keyword gKey, WReqBase request, string qValue);
    }
}
