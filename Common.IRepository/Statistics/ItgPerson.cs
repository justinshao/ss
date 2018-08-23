using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Statistics;

namespace Common.IRepository.Statistics
{
   public interface ItgPerson
   {
       #region 添加推广人员
       bool Add(tgPerson modle);
       #endregion

       #region 删除推广人员
       bool Delete(string tbn, string f, string v);
       #endregion
      
       #region 初始化
       string SearchTg();
       #endregion

       #region 更新推广人员
       bool Update(tgPerson modle);
        #endregion

        #region 推广人员统计 
        int Search_tgPersonStatisticsCount(InParams paras);
        List<tgPerson> Search_tgPersonStatistics(InParams paras);
        List<tgPerson> Search_tgPersonStatistics(InParams paras, int PageSize, int PageIndex);
        tgPerson QueryPersonByID(int id);
        #endregion

    }
}
