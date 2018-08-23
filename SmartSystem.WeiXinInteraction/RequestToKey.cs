using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinBase;
using Common.Entities.Enum;
using Common.Services.WeiXin;
using SmartSystem.WeiXinInteraction.ForKeywork;
using Common.Entities.WX;

namespace SmartSystem.WeiXinInteraction
{
    public class RequestToKey
    {
        public static IWRespBase GoGKey(WX_ApiConfig config, ReplyType rType, string qValue, WReqBase request)
        {
            var gkey = WXKeywordServices.QueryByReplyType(config.CompanyID,rType, qValue);
            if (gkey == null)
            {
                return null;
            }
            IKey iKey;
            switch (gkey.KeywordType)
            {
                // 文字
                case KeywordType.Text:
                    iKey = new ReplyText();
                    break;
                // 图文
                case KeywordType.Article:
                    iKey = new ReplyArticle();
                    break;
                default:
                    return null;
            }
            return iKey.ReplyContent(config,gkey, request, qValue);
        }
    }
}
