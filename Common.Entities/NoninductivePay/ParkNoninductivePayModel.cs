using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.NoninductivePay
{
    /// <summary>
    /// 车场无感支付类型
    /// </summary>
    public class ParkNoninductivePayModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 记录编号
        /// </summary>
        public string RecordID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        public string PKID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int DataStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 无感支付类型
        /// </summary>
        public int NoninductivePayType
        {
            get;
            set;
        }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence
        {
            get;
            set;
        }
    }
}
