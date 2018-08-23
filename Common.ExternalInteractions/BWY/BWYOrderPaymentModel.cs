using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExternalInteractions.BWY
{
    public class BWYOrderPaymentModel
    {
        /// <summary>
        /// 应缴费用
        /// </summary>
        public int FeeOfPayable { get; set; }
        /// <summary>
        /// 实缴费用
        /// </summary>
        public int FeeOfPaid { get; set; }
        /// <summary>
        /// 1 场内码，2 出口码
        /// </summary>
        public int CodeType { get; set; }
        /// <summary>
        /// 状态，1.未缴费；2.正在缴费；3.已缴费
        /// </summary>
        public int PayState { get; set; }
    }
    public class BWYOrderPaymentResult
    {
        /// <summary>
        /// 0-操作成功
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
    }
}
