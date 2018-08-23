using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository.Park
{
    public interface IParkDerate
    {
        bool Add(ParkDerate model);

        bool Update(ParkDerate model);

        bool Delete(string derateId);

        bool DeleteBySellerID(string sellerId);

        List<ParkDerate> QueryBySellerID(string sellerId);

        ParkDerate Query(string derateId);

        List<ParkDerateIntervar> QueryParkDerateIntervar(string derateId);
    }
}
