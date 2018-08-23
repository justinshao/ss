using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 进出分析模型
    /// </summary>
    public class InOutResult
    {
        List<Statistics_Gather> _dailytemp = new List<Statistics_Gather>();
        /// <summary>
        /// 近10天日临停量
        /// </summary>
        public List<Statistics_Gather> DailyTemp
        {
            get
            {
                return _dailytemp;
            }
            set
            {
                _dailytemp = value;
            }
        }
        List<Statistics_Gather> _monthtemp = new List<Statistics_Gather>();
        /// <summary>
        /// 近10个月日临停量
        /// </summary>
        public List<Statistics_Gather> MonthTemp
        {
            get
            {
                return _monthtemp;
            }
            set
            {
                _monthtemp = value;
            }
        }
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
        List<KeyValue> _cardtype = new List<KeyValue>();
        /// <summary>
        /// 进场类型
        /// </summary>
        public List<KeyValue> CardType
        {
            get
            {
                return _cardtype;
            }
            set
            {
                _cardtype = value;
            }
        }
        List<Statistics_Gather> _inoutpeak = new List<Statistics_Gather>();
        /// <summary>
        /// 进出分析
        /// </summary>
        public List<Statistics_Gather> InOutPeak
        {
            get
            {
                return _inoutpeak;
            }
            set
            {
                _inoutpeak = value;
            }
        }
    }
}
