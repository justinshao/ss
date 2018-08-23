using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXMenuAccessRecord
    {
        bool Create(WX_MenuAccessRecord model);
    }
}
