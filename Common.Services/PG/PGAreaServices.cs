using Common.Entities;
using Common.Entities.PG;
using Common.Factory.PG;
using Common.IRepository.PG;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.PG
{
    public class PGAreaServices
    {
        public static bool Add(PGArea model)
        {
            if (model == null) throw new ArgumentNullException("model");

            model.AreaID = GuidGenerator.GetGuid().ToString();
            IPGArea factory = PGAreaFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<PGArea>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(PGArea model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IPGArea factory = PGAreaFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<PGArea>(model, OperateType.Update);
            }
            return result;
        }
    }
}
