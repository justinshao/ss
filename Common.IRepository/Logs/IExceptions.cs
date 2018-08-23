using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.IRepository
{
    public interface IExceptions
    {
        void AddExceptions(Exceptions exceptions);

        List<Exceptions> LoadExceptions(ExceptionCondition condition, int pageIndex, int pageSize, out int recordTotalCount);
    }
}
