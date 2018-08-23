using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BaseData;
using Common.Entities;
using Common.IRepository;
using Common.Factory;
using Common.Utilities;

namespace Common.Services
{
    public class PassRemarkServices
    {

       public static bool Add(BasePassRemark model) {
           if (model == null) throw new ArgumentNullException("model");

           model.RecordID = GuidGenerator.GetGuidString();
           IPassRemark factory = PassRemarkFactory.GetFactory();
           bool result = factory.Add(model);
           if (result)
           {
               OperateLogServices.AddOperateLog<BasePassRemark>(model, OperateType.Add);
           }
           return result;
        }

       public static bool Update(BasePassRemark model)
       {
           if (model == null) throw new ArgumentNullException("model");

           IPassRemark factory = PassRemarkFactory.GetFactory();
           bool result = factory.Update(model);
           if (result)
           {
               OperateLogServices.AddOperateLog<BasePassRemark>(model, OperateType.Update);
           }
           return result;
        }

       public static bool Delete(string recordId)
       {
           if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

           IPassRemark factory = PassRemarkFactory.GetFactory();
           bool result = factory.Delete(recordId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
           }
           return result;
        }

       public static List<BasePassRemark> QueryByParkingId(string parkingId, PassRemarkType? passType)
       {
           if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

           IPassRemark factory = PassRemarkFactory.GetFactory();
           return factory.QueryByParkingId(parkingId, passType);
        }

       public static BasePassRemark QueryByRecordId(string recordId)
       {
           if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

           IPassRemark factory = PassRemarkFactory.GetFactory();
           return factory.QueryByRecordId(recordId);
        }
    }
}
