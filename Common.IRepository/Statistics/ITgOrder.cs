using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Statistics;

namespace Common.IRepository.Statistics
{
    public interface ITgOrder
    {
        bool addTgOrder(TgOrder modle);
        List<TgOrder> CountTgPersonOrder(InParams paras, int PageSize, int PageIndex);
        int CountTgPersonOrderCount(InParams paras);

        List<TgOrder> QueryAllCountTgPersonOrder(InParams paras);
    }
}
