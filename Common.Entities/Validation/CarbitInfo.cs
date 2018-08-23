using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Entities.Validation
{
    public class CarbitInfo
    { 
        /// <summary>
        /// 报警类型
        /// </summary>
        public CarbitAlarmType AlarmType { set; get; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber { set; get; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public DateTime AlarmTime { set; get; }
        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNo { set; get; }
        /// <summary>
        /// 报警图片
        /// </summary>
        public byte[] PicBuffer { set; get; }
    }

    public enum CarbitAlarmType
    {
        /// <summary>
        /// 有车
        /// </summary>
        [Description("有车")]
        HaveCar = 0,
        /// <summary>
        /// 无车
        /// </summary>
        [Description("无车")]
        NotCar = 1,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 2,
    }
}
