using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utilities;
using Common.Factory.Park;
using Common.IRepository.Park;

namespace Common.Services.Park
{
    public class ParkDerateServices
    {
        public static bool Add(ParkDerate model) {
            if (model == null) throw new ArgumentNullException("model");

            model.DerateID = GuidGenerator.GetGuidString();
            if (model.DerateIntervar != null && model.DerateIntervar.Count > 0) {
                model.DerateIntervar.ForEach(p => { p.DerateIntervarID = GuidGenerator.GetGuidString(); p.DerateID = model.DerateID; });
            }
            IParkDerate factory = ParkDerateFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerate>(model, OperateType.Add);
            }
            return result;
        }

       public static bool Update(ParkDerate model)
        {
            if (model == null) throw new ArgumentNullException("model");

            if (model.DerateIntervar != null && model.DerateIntervar.Count > 0)
            {
                model.DerateIntervar.ForEach(p => { p.DerateIntervarID = GuidGenerator.GetGuidString(); p.DerateID = model.DerateID; });
            }
            IParkDerate factory = ParkDerateFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerate>(model, OperateType.Update);
            }
            return result;
        }

       public static bool Delete(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           bool result = factory.Delete(derateId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("derateId:{0}", derateId));
           }
           return result;
       }

       public static bool DeleteBySellerID(string sellerId)
       {
           if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           bool result = factory.DeleteBySellerID(sellerId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("sellerId:{0}", sellerId));
           }
           return result;
       }

       public static List<ParkDerate> QueryBySellerID(string sellerId)
       {
           if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.QueryBySellerID(sellerId);
       }

       public static ParkDerate Query(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.Query(derateId);
       }

       public static List<ParkDerateIntervar> QueryParkDerateIntervar(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.QueryParkDerateIntervar(derateId);
       }
    }
}
