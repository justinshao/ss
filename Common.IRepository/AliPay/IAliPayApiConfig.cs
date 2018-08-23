using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.AliPay;

namespace Common.IRepository.AliPay
{
    public interface IAliPayApiConfig
    {
        bool Add(AliPayApiConfig model);

        bool Update(AliPayApiConfig model);

        AliPayApiConfig QueryByRecordID(string recordId);

        AliPayApiConfig QueryByCompanyID(string companyId);

        List<AliPayApiConfig> QueryAll();
    }
}
