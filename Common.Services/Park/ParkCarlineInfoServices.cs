using Common.Entities;
using Common.Factory;
using Common.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.Park
{
    public class ParkCarlineInfoServices
    {
        public static bool AddByGateid(ParkCarlineInfo model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkCarlineInfo factory = ParkCarlineInfoFactory.GetFactory();
            var tempmode = factory.QueryByGate(model.Gateid);
            bool result = false;
            if (tempmode == null)
            {
                result = factory.Add(model);
                if (result)
                {
                    OperateLogServices.AddOperateLog<ParkCarlineInfo>(model, OperateType.Add);
                }
            }
            else
            { 
                result = factory.Update(model);
                if (result)
                {
                    OperateLogServices.AddOperateLog<ParkCarlineInfo>(model, OperateType.Update);
                }
            }
            return result;
        }
        public static bool Delete(string gateid)
        {
            if (gateid.IsEmpty()) throw new ArgumentNullException("gateid");

            IParkCarlineInfo factory = ParkCarlineInfoFactory.GetFactory();
            return factory.Delete(gateid);
        }
        public static ParkCarlineInfo QueryMinTargetTimeInfo(string pkid)
        {
            if (pkid.IsEmpty()) throw new ArgumentNullException("pkid");

            IParkCarlineInfo factory = ParkCarlineInfoFactory.GetFactory();
            return factory.QueryMinTargetTimeInfo(pkid);  
        }
    }
}
