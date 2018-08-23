using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Statistics;
namespace Common.Entities.Statistics
{
    /// <summary>
    /// 首页数据模型
    /// </summary>
    public class HomeResult
    {
        List<KeyValue> _releasetype = new List<KeyValue>();
        /// <summary>
        /// 放行类型
        /// </summary>
        public List<KeyValue> ReleaseType
        {
            get
            {
                return _releasetype;
            }
            set
            {
                _releasetype = value;
            }
        }
        List<KeyValue> _entrancecardtype = new List<KeyValue>();
        /// <summary>
        /// 进场卡片类型
        /// </summary>
        public List<KeyValue> EntranceCardType
        {
            get
            {
                return _entrancecardtype;
            }
            set
            {
                _entrancecardtype = value;
            }
        }
        List<KeyValue> parktemptop5 = new List<KeyValue>();
        /// <summary>
        /// 车场临停前5
        /// </summary>
        public List<KeyValue> ParkTempTop5
        {
            get
            {
                return parktemptop5;
            }
            set
            {
                parktemptop5 = value;
            }
        }
        List<Statistics_Gather> _gathermonth12 = new List<Statistics_Gather>();
        /// <summary>
        /// 12月统计数据
        /// </summary>
        public List<Statistics_Gather> GatherMonth12
        {
            get
            {
                return _gathermonth12;
            }
            set
            {
                _gathermonth12 = value;
            }
        }

        List<Statistics_Gather> _gatherdaily30 = new List<Statistics_Gather>();
        /// <summary>
        /// 30天日统计
        /// </summary>
        public List<Statistics_Gather> GatherDaily30
        {
            get
            {
                return _gatherdaily30;
            }
            set
            {
                _gatherdaily30 = value;
            }
        }
    }
}
