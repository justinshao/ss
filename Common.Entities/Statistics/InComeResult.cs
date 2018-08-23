using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class InComeResult
    {
        List<Statistics_Gather> _mom = new List<Statistics_Gather>();
        /// <summary>
        /// 环比(本月与上年度的本月)
        /// </summary>
        public List<Statistics_Gather> MOM
        {
            get
            {
                return _mom;
            }
            set
            {
                _mom = value;
            }
        }
        List<Statistics_Gather> _yoy = new List<Statistics_Gather>();
        /// <summary>
        /// 同比(本月与上月)
        /// </summary>
        public List<Statistics_Gather> YOY
        {
            get
            {
                return _yoy;
            }
            set
            {
                _yoy = value;
            }
        }
        List<Statistics_Gather> _month12 = new List<Statistics_Gather>();
        /// <summary>
        /// 近12个月的统计数据
        /// </summary>
        public List<Statistics_Gather> GatherMonth12
        {
            get
            {
                return _month12;
            }
            set
            {
                _month12 = value;
            }
        }
    }
}
