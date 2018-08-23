using Common.Entities.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.WeiXin
{
    public interface IWXUser
    {
        WX_Info GetWXInfoByPlateNo(string sPlateNo);
    }
}
