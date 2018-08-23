using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Factory.WeiXin;
using Common.IRepository.WeiXin;

namespace Common.Services.WeiXin
{
    public class AdvanceParkingServices
    {
        public static bool Add(AdvanceParking model) {
            if (model == null) throw new ArgumentNullException("model");
            IAdvanceParking factory = AdvanceParkingFactory.GetFactory();
            return factory.Add(model);
        }

        public static AdvanceParking QueryByOrderId(decimal orderId)
        {
            IAdvanceParking factory = AdvanceParkingFactory.GetFactory();
            return factory.QueryByOrderId(orderId);
        }

        public static bool UpdatePrepayIdById(string prepayId, decimal orderId) {
            IAdvanceParking factory = AdvanceParkingFactory.GetFactory();
            return factory.UpdatePrepayIdById(prepayId,orderId);
        }

        public static bool PaySuccess(decimal orderId, string serialNumber, DateTime payTime) {
            IAdvanceParking factory = AdvanceParkingFactory.GetFactory();
            AdvanceParking model = factory.QueryByOrderId(orderId);
            if (model.OrderState != 0) throw new MyException("支付状态不正确");

           return factory.PaySuccess(orderId, serialNumber, payTime);
        }

        public static List<AdvanceParking> QueryPage(string companyId,string plateNo, DateTime? start, DateTime? end, int pageIndex, int pageSize, out int recordTotalCount)
        {
            IAdvanceParking factory = AdvanceParkingFactory.GetFactory();
            return factory.QueryPage(companyId,plateNo, start, end, pageIndex, pageSize, out recordTotalCount);
        }
    }
}
