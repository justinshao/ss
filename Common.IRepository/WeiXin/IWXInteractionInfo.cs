using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXInteractionInfo
    {
        bool Add(WX_InteractionInfo model);

        int QueryMaxIdByOpenId(string openId);
    }
}
