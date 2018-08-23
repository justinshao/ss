using Common.Entities.PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IRepository.PG
{
    public interface IPGArea
    {
        bool Add(PGArea model);

        bool Update(PGArea model);
    }
}
