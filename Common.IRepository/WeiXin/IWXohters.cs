using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXohters
    {

         WX_Info QueryByOpenId(string openid);

         string GetUrl(string plateno);
     
    }
}
