using System;
using Common.Utilities.Helpers;
using Common.Entities;
using Common.Entities.Parking;
using System.Collections.Generic;

namespace Common.Entities.Validation
{
    public class ResultAgs
    {
        //状态码
        public ResultCode ResCode { set; get; }

        /// <summary>
        /// 进出卡片基础类型
        /// </summary>
        public BaseCarType InOutBaseCardType { set; get; }

        /// <summary>
        /// 验证是否通过
        /// </summary>
        public ProcessValidInfo ProcessValidInfoRes { set; get; }

        /// <summary>
        /// 验证结果消息
        /// </summary>
        public string ValidMsg { set; get; }

        /// <summary>
        /// 优免结果消息
        /// </summary>
        public string DerateValidMsg { set; get; }
        /// <summary>
        /// 验证失败错误消息
        /// </summary>
        public string ValidFailMsg
        {
            get
            { 
                if (ResCode == ResultCode.Defaut
                    || ResCode == ResultCode.InOK
                    || ResCode == ResultCode.OutOK)
                {
                    return "";
                }
                return EnumHelper.GetDescription(ResCode);
            }
        }

        /// <summary>
        /// 进时间
        /// </summary>
        public DateTime InDate { set; get; }

        /// <summary>
        /// 出时间
        /// </summary>
        public DateTime OutDate { set; get; }

        /// <summary>
        /// 收费信息
        /// </summary>
        public RateInfo Rate { set; get; }

        /// <summary>
        /// 是否已收费
        /// </summary>
        public int IsFree { set; get; }

        /// <summary>
        /// 是否免费
        /// </summary>
        public bool IsMF { set; get; } 
        /// 是否军警免费车辆
        /// </summary>
        public bool IsPoliceFree { set; get; }
         
        /// <summary>
        /// 免费原因
        /// </summary>
        public string MFMsg { set; get; }

        /// <summary>
        /// 车位占用时 固定车的临停时段；
        /// </summary>
        public TimeSpan OverdueToTempSpan { set; get; }

        /// <summary>
        /// 是否月卡转临停车
        /// </summary>
        public bool IsOverdueToTemp { set; get; }

        /// <summary>
        ///通行类型 类型 0 正常 1 固定转临停 2 车位占用转临停 
        ///入场的时候需要写入PKIORecord 出场仅记录类型
        /// </summary>
        public int EnterType { set; get; }

        /// <summary>
        /// 访客车辆
        /// </summary>
        public bool IsVisitorCar { set; get; }
        /// <summary>
        /// 访客车辆
        /// </summary>
        public ParkVisitor VisitorCar { set; get; }

        /// <summary>
        /// 预约车辆
        /// </summary>
        public bool IsReserveCar { set; get; }
         
        /// <summary>
        /// 车辆预约信息
        /// </summary>
        public ParkReserveBit  ReserveBit { set; get; }
        /// <summary>
        /// 用于算费的规则
        /// </summary>
        public ParkFeeRule FeeRule { set; get; }

        /// <summary>
        /// 优免券信息
        /// </summary>
        public List<ParkCarDerate> Carderates { set; get; }
    }
}
