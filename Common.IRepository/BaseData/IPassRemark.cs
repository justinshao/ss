using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BaseData;
using Common.Entities;

namespace Common.IRepository
{
    public interface IPassRemark
    {
        List<BasePassRemark> QueryByParkingId(string parkingId, PassRemarkType? passType);

        bool Add(BasePassRemark model);

        bool Update(BasePassRemark model);

        bool Delete(string recordId);

        BasePassRemark QueryByRecordId(string recordId);
    }
}
